$(function () {

    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click( () => {
            if (!confirm("Do you realy want to delete it?")) return false;
        });
    }

    if ($("div.alert.notification").length) {
        setTimeout( () => {
            $("div.alert.notification").fadeOut();
        }, 2000);
    }
});

function readUrl(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $("img#imgPreview")
                .attr("src", e.target.result)
                .width(200)
                .height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}