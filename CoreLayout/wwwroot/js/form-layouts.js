$(function() {
	'use strict'
	$('.main-form-group .form-control').on('focusin focusout', function() {
		$(this).parent().toggleClass('focus');
	});
	$(document).ready(function() {
		$('.select2').select2({
			width: '100%',
			placeholder: 'Choose one'
		});
		$('.select2-no-search').select2({
			minimumResultsForSearch: Infinity,
			width: '100%',
			placeholder: 'Choose one'
		});
	});
});