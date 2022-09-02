jQuery(function (e) {
    'use strict';
    $(document).ready(function () {
      $('#summernote').summernote();
    });
    $('#summernote').summernote({
      placeholder: 'Hello bootstrap 4',
      tabsize: 3,
      height: 350
    });
  });