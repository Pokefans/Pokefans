// Copyright 2016 the pokefans authors. See copying.md for legal info.
if (typeof (Dropzone) != undefined) {
    Dropzone.autoDiscover = false;
    $(function () {
        var uri = $('#fanartUploadForm').attr('action');
        var myDropzone = new Dropzone("#fanartUploadImage",
        {
            url: uri,
            paramName: "file",
            maxFilesize: 1, //MB
            acceptedFiles: 'image/*', // allow only images
            dictDefaultMessage: "Ziehe eine Datei per Drag & Drop hier hin oder klicke hier, um sie hochzuladen.",
            dictFallbackMessage: "Leider unterstützt dein Browser kein Drag & Drop von Dateien.",
            dictFallbackText: "Bitte benutze den traditionellen Uploader weiter unten.",
            dictInvalidFileType: "Du kannst nur Bilddateien hochladen. Erlaubt sind (u.A.): JPG, PNG, BMP, GIF",
            dictFileTooBig: "Dein Bild ({{filesize}}) ist zu groß. Die maximal erlaubte Dateigröße beträgt {{maxFilesize}}. Je nach Kategorie kann diese niedriger ausfallen!",
            autoProcessQueue: false,

        });

        myDropzone.on("sending", function (file, xhr, formData) {
            formData.append("categoryId", $('#Category').val());
            var selectedVal = 0;
            var selected = $("input[type='radio'][name='License']:checked");
            if (selected.length > 0) {
                selectedVal = selected.val();
            }
            formData.append("licenseId", selectedVal);
        });

        myDropzone.on("success", function (file, response) {
            window.location.href = '/verwaltung/einreichung/' + response; // successfully uploaded, redirect
        });
        $('#send').click(function (e) {
            e.preventDefault();

            myDropzone.processQueue();
        });

        $("#clearbtn").click(function (e) {
            e.preventDefault();

            var i = 0;
            for (i = 0; i < myDropzone.files.length; i++) {
                myDropzone.removeFile(myDropzone.files[0]);
            }
        });
    });
}