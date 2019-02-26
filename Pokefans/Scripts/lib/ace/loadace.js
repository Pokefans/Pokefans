    var editors = new Array();
    $(function () {
        function getCookie(cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for(var i=0; i<ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0)==' ') c = c.substring(1);
                if (c.indexOf(name) != -1) return c.substring(name.length,c.length);
            }
            return "";
        }
        var isDark = false;
        if(getCookie("lightswitch") == "dark")
            isDark = true;
        $('textarea[data-editor]').each(function () {
            var textarea = $(this);
 
            var mode = textarea.data('editor');
 
            var editDiv = $('<div>', {
                position: 'absolute',
                style: 'height:500px;',
                'class': "content-editor"
            }).insertBefore(textarea);
 
            textarea.css('display', 'none');
 
            var editor = ace.edit(editDiv[0]);
            editor.getSession().setValue(textarea.val());
            editor.getSession().setMode("ace/mode/" + mode);
            editor.getSession().setUseSoftTabs(true);
            editor.getSession().setTabSize(4);
            editor.getSession().setUseWrapMode(true);
            editor.setHighlightActiveLine(true);
            editor.setShowPrintMargin(false);
            if(!isDark)
                editor.setTheme("ace/theme/clouds");
            else
                editor.setTheme("ace/theme/monokai");
            
            editors[editors.length] = editor;

            // copy back to textarea on form submit...
            textarea.closest('form').submit(function () {
                textarea.val(editor.getSession().getValue());
            });
 
        });
        if($("#lightswitch").length) {
            $('#lightswitch').click(function()
            {
                
                if(isDark)
                {

                    isDark = false;
                    for(i = 0; i < editors.length; i++)
                    {
                        editors[i].setTheme("ace/theme/clouds");
                    }
                }
                else
                {
                    for(i = 0; i < editors.length; i++)
                    {
                        editors[i].setTheme("ace/theme/monokai");
                    }
                    isDark = true;
                }   
            });
        }

        // Für folgende User keinen Shortcut für "Ctrl-Alt-0" anlegen, da Überschneidungen mit "}"
        var noUnfoldShortcutUsers = ['44021', '756'];
        if(noUnfoldShortcutUsers.indexOf(getCookie('user_id')) != -1)
        {
            for(i = 0; i < editors.length; i++) { editors[i].commands.addCommand({ name: "nofold", bindKey: { win: "Ctrl-Alt-0", mac: "Ctrl-Command-Option-0" }, exec: function() { return false; }, readOnly: true}); }
        }
    });