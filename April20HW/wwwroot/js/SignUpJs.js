$(() => {
    $("input").on('keyup', function () {
        enable();
    })
    function isFilled() {
        const name = $("#Name").val();
        const email = $("#Email").val();
        const password = $("#Password").val();
        return name && email && password;
    }
    function enable() {
        $("#btn").prop('disabled', !isEmail());
    }
    function isEmail() {
        const email = $("#Email").val();
        if (isFilled() && email.indexOf("@") !== -1) {
            $("#invalid").prop('hidden', true)
            return true;
        }
        else if (isFilled() && email.indexOf("@") === -1) {
            $("#invalid").text('Enter a valid email');
            $("#invalid").prop('hidden', false)
            return false;
        }
    }

})