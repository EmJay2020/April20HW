$(() => {
    $("#search").on('keyup', function () {
        const text = $(this).val();
        $(".container .well").each(function () {
            const well = $(this);
            const title = well.find("h4:eq(0)").text();
            console.log(title)
            if (title.toLowerCase().indexOf(text.toLowerCase()) !== -1) {
                well.show();
            } else {
                well.hide();
                 
            }
        })
    })
})