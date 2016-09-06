// Copyright 2016 the pokefans authors. See copying.md for legal info.
/**
 * Verhält sich genau wie nl2br()
 */
function nl2br(str) {
    return str.replace(/(\r\n|\r|\n)/g, '<br />$1');
}

/**
 * Fügt ein Kommentar ein.
 *
 * data ist ein Objekt mit den Eigenschaften "context", "commentedObjectId", "parentId", "commentId", "text", "author" und "submitTime"
 */
function insertComment(comment) {
    // #ID für Kommentar-Anker setzen
    $commentTemplate = $('[data-role="commentTemplate"]').clone().attr('id', 'k' + comment.CommentId).removeClass('hidden').removeAttr('data-role');

    // Den Anker zum Kommentar setzen
    $commentTemplate.find('a[data-role="commentLink"]').attr('href', '#k' + comment.CommentId);

    // Den Autor des Kommentars setzen
    $commentTemplate.find('[data-role="commentAuthor"]').html(comment.Author);

    // Wenn der Autor einen Mini-Avatar hat, soll der ebenfalls angezeigt werden
    if (comment.avatarFilename) {
        var $miniAvatar = $commentTemplate.find('[data-role="miniAvatar"]');
        $miniAvatar.attr('src', $miniAvatar.data('avatarurl') + comment.AvatarFileName);
    }

    // Kommentar-Erstellzeit
    $commentTemplate.find('[data-role="commentTime"]').text(comment.SubmitTimeString);

    // Sämtliche Felder mit der Kommentar#ID füllen
    $commentTemplate.find('[data-commentid]').attr('data-commentid', comment.CommentId);

    // Kommentar-Inhalt
    $commentTemplate.find('[data-role="commentText"]').html(nl2br(comment.Text));

    // Den Wrapper für zukünftige Kommentare vorbereiten
    $commentTemplate.find('[data-role="commentWrapper"]')
        .attr('data-parentid', comment.CommentId)
        .attr('data-context', comment.Context)
        .attr('data-commentedobjectid', comment.CommentedObjectId);

    if (!comment.IsDeletable) {
        $commentTemplate.find('[data-role="deleteComment"]').remove();
    }

    // Wenn das Level des Kommentars zu groß wird, soll nicht mehr verschachtelt werden
    var commentNesting = (comment.Level <= 2) ? '1' : '0';

    // Den Link zum Beantworten der Kommentare mit den nötigen Daten befüllen
    $commentTemplate.find('[data-role="commentAnswer"]')
        .attr('data-commentedobjectid', comment.CommentedObjectId)
        .attr('data-context', comment.Context);

    // Die #ID in die URLs zum Verstecken und Löschen der Kommentare einfügen
    $commentTemplate.find('[data-role="toggleHide"]').attr('href', $commentTemplate.find('[data-role="toggleHide"]').attr('href') + '&commentId=' + comment.CommentId);
    $commentTemplate.find('[data-role="deleteComment"]').attr('href', $commentTemplate.find('[data-role="deleteComment"]').attr('href') + '&commentId=' + comment.CommentId);

    var parent = comment.ParentCommentId == null ? 0 : comment.ParentCommentId;
    $commentWrapper = $('body').find('[data-role="commentWrapper"][data-commentnesting="' + commentNesting + '"][data-context="' + comment.Context + '"][data-commentedobjectid="' + comment.CommentedObjectId + '"][data-parentid="' + parent + '"]');

    // Ggf. den Hinweis "Noch keine Kommentare" löschen
    $commentWrapper.find('[data-role="noCommentsWarning"]').remove();

    $commentWrapper.prepend($commentTemplate);
}


/**
 * Fügt einen Kommentarbaum ein
 */
function insertCommentTree(comments) {
    if (comments.length == 0) {
        return;
    }

    // Das Array muss hier umgedreht werden, weil die Kommentare mit insertComment() von unten nach oben eingefügt werden.
    // Allerdings ist comments dafür in der falschen Reihenfolge.
    $.each(comments.reverse(), function (key, comment) {
        insertComment(comment);
        insertCommentTree(comment.childComments);
    });
}


$(document).ready(function (e) {
    var deleteDialog = $('<div class="modal fade" id="deleteDialog" tabindex="-1" role="dialog"><div class="modal-dialog modal-sm"><div class="modal-content"><div class="modal-body"><p>Willst du wirklich diesen Kommentar löschen?</p><a href="" class="btn btn-danger" data-role="confirmDelete" data-commentid="">Kommentar löschen</a> <button type="button" class="btn btn-default" data-dismiss="modal">Abbrechen</button></div></div></div></div>');
    $('#content').append(deleteDialog);

    //
    // Wird aufgerufen, wenn auf den Antworten-Link geklickt wird
    //
    $('body').on('click', '[data-role="commentAnswer"]', function (e) {
        e.preventDefault();

        var $link = $(this);

        var commentId = $link.data('commentid');
        var commentedObjectId = $link.data('commentedobjectid');
        var context = $link.data('context');

        var $commentFormContainer = $('#k' + commentId).find('[data-role="replyForm"][data-commentid="' + commentId + '"]');

        // Wird auf den Link geklickt und kein Formular ist sichtbar, dann soll es angezeigt werden.
        // Umgekehrt soll das Formular versteckt werden, wenn es bereits sichtbar ist.
        if ($commentFormContainer.children().length == 0) {
            var $commentForm = $('[data-role="rootReplyForm"][data-commentedobjectid="' + commentedObjectId + '"][data-context="' + context + '"]').clone();

            $commentForm.find('[name="parentId"]').val(commentId);
            $commentForm.find('[name="commentedObjectId"]').val(commentedObjectId);
            $commentForm.find('[name="context"]').val(context);
            $commentForm.find('[data-role="submitError"]').removeClass('hide').addClass('hide');

            $commentFormContainer.html($commentForm);
        }
        else {
            $commentFormContainer.html('');
        }
    });


    //
    // Wird aufgerufen, wenn ein Kommentar-Formular abgesendet wird
    //
    $('body').on('click', '[data-role="commentSubmit"]', function (e) {
        e.preventDefault();

        var $button = $(this);
        var $form = $button.closest('form');
        var $submitErrorBox = $form.find('[data-role="submitError"]');


        // Wird über AJAX abgesendet, also Formularwert entsprechend setzen
        $form.find('[name="ajax"]').val('1');

        var parentId = $form.find('[name="parentId"]').val();

        // Spinner-Icon anzeigen und Button deaktivieren
        var buttonText = $button.html();
        $button.prepend('<i class="fa fa-spinner fa-pulse"></i> ').attr('disabled', '');

        $.ajax({
            type: 'post',
            url: $form.attr('action'),
            data: $form.serialize(),

            success: function (data) {
                // Button wiederherstellen
                $button.html(buttonText);
                $button.removeAttr('disabled');

                if (data.Success) {
                    insertComment(data.Comment);

                    // Das Textfeld leeren
                    $form.find('[name="text"]').val('');
                    $submitErrorBox.removeClass('hide').addClass('hide');

                    // Formular ausblenden, wenn es nicht das Hauptformular ist
                    if (parentId != 0) {
                        $form.remove();
                    }
                }
                else {
                    $submitErrorBox.removeClass('hide').html(data.error);
                }
            },

            xhrFields: {
                withCredentials: true
            }
        });
    });


    //
    // Wird aufgerufen, wenn ein Kommentar versteckt werden soll
    //
    $('body').on('click', '[data-role="toggleHide"]', function (e) {
        e.preventDefault();

        var $link = $(this);
        var $commentText = $(this);

        var commentId = $link.data('commentid');

        var $commentTextContainer = $('body').find('[data-role="commentText"][data-commentid="' + commentId + '"]');

        $.ajax({
            type: 'get',
            url: $link.attr('href'),
            data: { ajax: '1' },

            success: function (data) {
                if (data.success) {
                    if (!data.commentDisplay) {
                        $commentTextContainer.html('<span class="text-muted">Der Inhalt dieses Kommentars wurde von der Moderation ausgeblendet</span>');
                    }
                    else {
                        $commentTextContainer.html(nl2br(data.commentText));
                    }
                }
            },

            xhrFields: {
                withCredentials: true
            }
        });
    });


    //
    // Wird aufgerufen, wenn ein Kommentar gelöscht werden soll
    //
    $('body').on('click', '[data-role="deleteComment"]', function (e) {
        e.preventDefault();

        var $link = $(this);

        var commentId = $link.data('commentid');

        $('#deleteDialog').find('[data-role="confirmDelete"]').attr('href', $link.attr('href')).attr('data-commentid', commentId);
        $('#deleteDialog').modal();
    });


    //
    // Wird aufgerufen, wenn im Bestätigungsdialog auf "Löschen" geklickt wurde
    //
    $('body').on('click', '[data-role="confirmDelete"]', function (e) {
        e.preventDefault();

        var $this = $(this);
        var commentId = $this.data('commentid');

        $.ajax({
            type: 'get',
            url: $this.attr('href'),
            data: { ajax: '1' },

            success: function (data) {
                if (data === true) {
                    $('#k' + commentId).remove();
                    $('#deleteDialog').modal('hide');
                }
            },

            xhrFields: {
                withCredentials: true
            }
        });
    });


    //
    // Wird aufgerufen, wenn auf einen Link geklickt wird, um die Kommentarliste über AJAX zu laden
    //
    $('body').on('click', '[data-role="toggleComments"]', function (e) {
        e.preventDefault();

        var $link = $(this);
        var commentedObjectId = $link.data('commentedobjectid');

        var $commentBox = $('body').find('.box-comments').has('[data-role="commentWrapper"][data-commentedobjectid="' + commentedObjectId + '"]');
        var $commentWrapper = $('body').find('[data-role="commentWrapper"][data-commentedobjectid="' + commentedObjectId + '"][data-context="' + $link.data('context') + '"]');

        // Den Ladevorgang anzeigen
        $link.blur().find('i').removeClass('fa-comments').addClass('fa-spinner').addClass('fa-pulse');

        $commentBox.removeClass('hide');

        if (!$link.attr('disabled')) {
            $link.attr('disabled', '');

            $.ajax({
                method: 'get',
                url: $link.data('loadurl'),
                data: { ajax: '1', context: $commentWrapper.data('context'), commentedObjectId: $commentWrapper.data('commentedobjectid') },

                success: function (data) {
                    $link.remove();

                    if (data.comments.length > 0) {
                        insertCommentTree(data.comments);
                    }
                    else {
                        $commentWrapper.append('<div class="alert alert-warning" style="margin-top: 10px;" data-role="noCommentsWarning">Es wurden noch keine Kommentare erstellt!</div>');
                    }
                },

                xhrFields: {
                    withCredentials: true
                }
            });
        }
    });
});