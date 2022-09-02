$(function() {
	'use strict'
	
	// colored tooltip
	$('[data-bs-toggle="tooltip-primary"]').tooltip({
		template: '<div class="tooltip tooltip-primary" role="tooltip"><div class="tooltip-arrow"><\/div><div class="tooltip-inner"><\/div><\/div>'
	});
	$('[data-bs-toggle="tooltip-secondary"]').tooltip({
		template: '<div class="tooltip tooltip-secondary" role="tooltip"><div class="tooltip-arrow"><\/div><div class="tooltip-inner"><\/div><\/div>'
	});
	$('[data-bs-toggle="tooltip-info"]').tooltip({
		template: '<div class="tooltip tooltip-info" role="tooltip"><div class="tooltip-arrow"><\/div><div class="tooltip-inner"><\/div><\/div>'
	});
	$('[data-bs-toggle="tooltip-danger"]').tooltip({
		template: '<div class="tooltip tooltip-danger" role="tooltip"><div class="tooltip-arrow"><\/div><div class="tooltip-inner"><\/div><\/div>'
	});
	$('[data-bs-toggle="tooltip-success"]').tooltip({
		template: '<div class="tooltip tooltip-success" role="tooltip"><div class="tooltip-arrow"><\/div><div class="tooltip-inner"><\/div><\/div>'
	});
	$('[data-bs-toggle="tooltip-warning"]').tooltip({
		template: '<div class="tooltip tooltip-warning" role="tooltip"><div class="tooltip-arrow"><\/div><div class="tooltip-inner"><\/div><\/div>'
	});
});