$(() => {
    $("input").on('keyup', function () {
        enable();
    })
    function isFilled() {
        
        const email = $("#Email").val();
        const password = $("#Password").val();
        return  email && password;
    }
    function enable() {
        $("#btn").prop('disabled', !isFilled());
    }
})