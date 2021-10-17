$(function () {

    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click( () => {
            if (!confirm("Do you realy want to delete this page?")) return false;
        });
    }

    if ($("div.alert.notification").length) {
        setTimeout( () => {
            $("div.alert.notification").fadeOut();
        }, 2000);
    }
});