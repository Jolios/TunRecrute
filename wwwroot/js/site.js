$(function () {
    var placeholderElement = $('#modal-placeholder');
    $(document).on('click', 'button[data-toggle="ajax-modal"], a[data-toggle="ajax-modal"]', function (event) {
        event.preventDefault();
        var url = $(this).data('url');
        var placeholderElement = $('#modal-placeholder-'.concat(event.currentTarget.id));

        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
            feather.replace();
        });
    });
    $(document).on('click', 'button[data-save="modal"]', function (event) {
        console.log("in");
        event.preventDefault();
        var placeholderElement = $('#modal-placeholder-'.concat(event.currentTarget.id.toLowerCase()));
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            console.log("in");
            console.log(placeholderElement.html());
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            // find IsValid input field and check it's value
            // if it's valid then hide modal window
            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                placeholderElement.find('.modal').modal('hide');
                window.location.reload();
            }
        });
    });
});