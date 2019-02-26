// copyright 2018 the pokefans authors. see copying.md for details
var notificationsOut = false;
$('#userbar-notification-button').click(function(ev) {
    var e=$(this);

    ev.preventDefault();

    if(notificationsOut) {
        e.popover("dispose");
        notificationsOut = false;
    }
    else {
        var foo = e.data('poload');
        
        $.ajax({
            url: foo, 
            xhrFields: {
                withCredentials: true
            }, 
            success: function(d) {
                e.popover({
                    content: d, 
                    html: true, 
                    placement: "top", 
                    toggle: "manual", 
                    title: "Benachrichtigungen", 
                    template: '<div class="popover popover-notifications" role="tooltip"><div class="arrow"></div><h3 class="popover-header"></h3><div class="popover-body"></div></div>'
                }).popover('show');
                notificationsOut = true;
            }
        });
    }
});
