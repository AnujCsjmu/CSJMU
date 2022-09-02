$(function () {
    'use strict'

    // ______________ Page Loading
    $("#global-loader").fadeOut("slow");

    // ______________ Card
    const DIV_CARD = 'div.card';

    // ______________ Function for remove card
    $(document).on('click', '[data-bs-toggle="card-remove"]', function (e) {
        let $card = $(this).closest(DIV_CARD);
        $card.remove();
        e.preventDefault();
        return false;
    });

    // ______________ Functions for collapsed card
    $(document).on('click', '[data-bs-toggle="card-collapse"]', function (e) {
        let $card = $(this).closest(DIV_CARD);
        $card.toggleClass('card-collapsed');
        e.preventDefault();
        return false;
    });


    // ______________ Card full screen
    $(document).on('click', '[data-bs-toggle="card-fullscreen"]', function (e) {
        let $card = $(this).closest(DIV_CARD);
        $card.toggleClass('card-fullscreen').removeClass('card-collapsed');
        e.preventDefault();
        return false;
    });

    // ______________ COVER IMAGE
    $(".cover-image").each(function () {
        var attr = $(this).attr('data-bs-image-src');
        if (typeof attr !== typeof undefined && attr !== false) {
            $(this).css('background', 'url(' + attr + ') center center');
        }
    });

    // ______________Main-navbar
    if (window.matchMedia('(min-width: 992px)').matches) {
        $('.main-navbar .active').removeClass('show');
        $('.main-header-menu .active').removeClass('show');
    }
    $('.main-header .dropdown > a').on('click', function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('show');
        $(this).parent().siblings().removeClass('show');
    });
    $('.main-navbar .with-sub').on('click', function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('show');
        $(this).parent().siblings().removeClass('show');
    });
    $('.main-navbar .with-sub1').on('click', function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('show');
        $(this).parent().siblings().removeClass('show');
    });
    $('.main-navbar .with-sub2').on('click', function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('show');
        $(this).parent().siblings().removeClass('show');
    });
    $('.main-navbar .with-sub3').on('click', function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('show');
        $(this).parent().siblings().removeClass('show');
    });
    $('.dropdown-menu .main-header-arrow').on('click', function (e) {
        e.preventDefault();
        $(this).closest('.dropdown').removeClass('show');
    });
    $('#mainNavShow, #azNavbarShow').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('main-navbar-show');
    });
    $('#mainContentLeftShow').on('click touch', function (e) {
        e.preventDefault();
        $('body').addClass('main-content-left-show');
    });
    $('#mainContentLeftHide').on('click touch', function (e) {
        e.preventDefault();
        $('body').removeClass('main-content-left-show');
    });
    $('#mainContentBodyHide').on('click touch', function (e) {
        e.preventDefault();
        $('body').removeClass('main-content-body-show');
    })

    // ______________Dropdown menu
    $(document).on('click touchstart', function (e) {
        e.stopPropagation();
        var dropTarg = $(e.target).closest('.main-header .dropdown').length;
        if (!dropTarg) {
            $('.main-header .dropdown').removeClass('show');
        }
        if (window.matchMedia('(min-width: 992px)').matches) {
            var navTarg = $(e.target).closest('.main-navbar .nav-item').length;
            if (!navTarg) {
                $('.main-navbar .show').removeClass('show');
            }
            var menuTarg = $(e.target).closest('.main-header-menu .nav-item').length;
            if (!menuTarg) {
                $('.main-header-menu .show').removeClass('show');
            }
            if ($(e.target).hasClass('main-menu-sub-mega')) {
                $('.main-header-menu .show').removeClass('show');
            }
        } else {
            if (!$(e.target).closest('#mainMenuShow').length) {
                var hm = $(e.target).closest('.main-header-menu').length;
                if (!hm) {
                    $('body').removeClass('main-header-menu-show');
                }
            }
        }
    });

    // ______________MainMenuShow
    $('#mainMenuShow').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('main-header-menu-show');
    })
    $('.main-header-menu .with-sub').on('click', function (e) {
        e.preventDefault();
        $(this).parent().toggleClass('show');
        $(this).parent().siblings().removeClass('show');
    })
    $('.main-header-menu-header .close').on('click', function (e) {
        e.preventDefault();
        $('body').removeClass('main-header-menu-show');
    })

    // ______________Tooltip
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
    // ______________Popover
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    })
    // ______________Toast
    $(".toast").toast();
    // ______________ Toast
    var toastElList = [].slice.call(document.querySelectorAll('.toast'))
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl)
    })


    // ______________ Page Loading
    $(window).on("load", function (e) {
        $("#global-loader").fadeOut("slow");
    })

    // ______________Back-top-button
    $(window).on("scroll", function (e) {
        if ($(this).scrollTop() > 0) {
            $('#back-to-top').fadeIn('slow');
        } else {
            $('#back-to-top').fadeOut('slow');
        }
    });
    $(document).on("click", "#back-to-top", function (e) {
        $("html, body").animate({
            scrollTop: 0
        }, 0);
        return false;
    });

    // ______________Full screen
    $(document).on("click", ".fullscreen-button", function toggleFullScreen() {
        $('html').addClass('fullscreenie');
        if ((document.fullScreenElement !== undefined && document.fullScreenElement === null) || (document.msFullscreenElement !== undefined && document.msFullscreenElement === null) || (document.mozFullScreen !== undefined && !document.mozFullScreen) || (document.webkitIsFullScreen !== undefined && !document.webkitIsFullScreen)) {
            if (document.documentElement.requestFullScreen) {
                document.documentElement.requestFullScreen();
            } else if (document.documentElement.mozRequestFullScreen) {
                document.documentElement.mozRequestFullScreen();
            } else if (document.documentElement.webkitRequestFullScreen) {
                document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
            } else if (document.documentElement.msRequestFullscreen) {
                document.documentElement.msRequestFullscreen();
            }
        } else {
            $('html').removeClass('fullscreenie');
            if (document.cancelFullScreen) {
                document.cancelFullScreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.webkitCancelFullScreen) {
                document.webkitCancelFullScreen();
            } else if (document.msExitFullscreen) {
                document.msExitFullscreen();
            }
        }
    })

    // ______________Cover Image
    $(".cover-image").each(function () {
        var attr = $(this).attr('data-image-src');
        if (typeof attr !== typeof undefined && attr !== false) {
            $(this).css('background', 'url(' + attr + ') center center');
        }
    });
    

    // OFF-CANVAS STYLE
    $('.off-canvas').on('click', function () {
        $('body').addClass('overflow-y-scroll');
        $('body').addClass('pe-0');
    });


    function replay() {
        let replayButtom = document.querySelectorAll('.reply a')
        // Creating Div
        let Div = document.createElement('div')
        Div.setAttribute('class', "comment mt-4 d-grid")
        // creating textarea
        let textArea = document.createElement('textarea')
        textArea.setAttribute('class', "form-control")
        textArea.setAttribute('rows', "5")
        textArea.innerText = "Your Comment";
        // creating Cancel buttons
        let cancelButton = document.createElement('button');
        cancelButton.setAttribute('class', "btn btn-danger");
        cancelButton.innerText = "Cancel";

        let buttonDiv = document.createElement('div')
        buttonDiv.setAttribute('class', "btn-list ms-auto mt-2")

        // Creating submit button
        let submitButton = document.createElement('button');
        submitButton.setAttribute('class', "btn btn-success");
        submitButton.innerText = "Submit";

        // appending text are to div
        Div.append(textArea)
        Div.append(buttonDiv);
        buttonDiv.append(cancelButton);
        buttonDiv.append(submitButton);

        replayButtom.forEach((element, index) => {

            element.addEventListener('click', () => {
                let replay = $(element).parent()
                replay.append(Div)

                cancelButton.addEventListener('click', () => {
                    Div.remove()
                })
            })
        })


    }
    replay()


    // ______________ SWITCHER-toggle ______________//
    
    $('.layout-setting').on("click", function (e) {
        if (!(document.querySelector('body').classList.contains('dark-theme'))) {
            $('body').addClass('dark-theme');
            $('body').removeClass('light-theme');
            $('body').removeClass('transparent-theme');

            $('#myonoffswitch5').prop('checked', true);
            $('#myonoffswitch8').prop('checked', true);

            localStorage.setItem('darkMode', true);
            localStorage.removeItem('lightMode');
            localStorage.removeItem('transparentMode');
            $('#myonoffswitch2').prop('checked', true);
        } else {
            $('body').removeClass('dark-theme');
            $('body').addClass('light-theme');
            $('#myonoffswitch3').prop('checked', true);
            $('#myonoffswitch6').prop('checked', true);

            localStorage.setItem('lightMode', true);
            localStorage.removeItem('transparentMode');
            localStorage.removeItem('darkMode');;
            $('#myonoffswitch1').prop('checked', true);
        }
    });
    /******* Theme Style ********/

	//---- Light theme ----- //
	$('body').addClass('light-theme');
	$('body').removeClass('transparent-theme');
	$('body').removeClass('dark-theme');

	//---- Dark theme ----- //
	// $('body').addClass('dark-theme');
	// $('body').removeClass('light-theme');
	// $('body').removeClass('transparent-theme');

	//---- Transparent theme ----//
	// $('body').addClass('transparent-theme');
	// $('body').removeClass('light-theme');
	// $('body').removeClass('dark-theme');

    /******* Transparent Bg-Image Style *******/

	// Bg-Image1 Style
	// $('body').addClass('bg-img1');
	// $('body').addClass('transparent-theme');
	// $('body').removeClass('light-theme');
	// $('body').removeClass('dark-theme');

	// Bg-Image2 Style
	// $('body').addClass('bg-img2');
	// $('body').addClass('transparent-theme');
	// $('body').removeClass('light-theme');
	// $('body').removeClass('dark-theme');

	// Bg-Image3 Style
	// $('body').addClass('bg-img3');
	// $('body').addClass('transparent-theme');
	// $('body').removeClass('light-theme');
	// $('body').removeClass('dark-theme');

	// Bg-Image4 Style
	// $('body').addClass('bg-img4');
	// $('body').addClass('transparent-theme');
	// $('body').removeClass('light-theme');
	// $('body').removeClass('dark-theme');

    /******* Header Styles ********/

	// $('body').addClass('light-header');
	// $('body').addClass('color-header');
	// $('body').addClass('dark-header');
	// $('body').addClass('gradient-header');


	/******* Menu Styles ********/

	// $('body').addClass('light-menu');	
	// $('body').addClass('color-menu');
	// $('body').addClass('dark-menu');
	// $('body').addClass('gradient-menu');


	/******* Full Width Layout Start ********/

	// $('body').addClass('layout-boxed'); 
	

	/******** *Header-Position Styles Start* ********/

	// $('body').addClass('scrollable-layout');

	// Sidemenu layout Styles //

	// ***** Icon with Text *****//
        // $('body').addClass('icontext-menu');
        // $('body').addClass('main-sidebar-hide');
        // if(document.querySelector('.icontext-menu').firstElementChild.classList.contains('login-img') !== true){
        // icontext();
        // }

	// ***** Icon Overlay ***** //
        // $('body').addClass('icon-overlay');
        // $('body').addClass('main-sidebar-hide');

	// ***** closed-menu ***** //
        // $('body').addClass('closed-menu');
        // $('body').addClass('main-sidebar-hide')

	// ***** hover-submenu ***** //
        // $('body').addClass('hover-submenu');
        // $('body').addClass('main-sidebar-hide')
        // if(document.querySelector('.hover-submenu').firstElementChild.classList.contains('login-img') !== true){
        // hovermenu();
        // }

	// ***** hover-submenu style 1 ***** //
        // $('body').addClass('hover-submenu1');
        // $('body').addClass('main-sidebar-hide')
        // if(document.querySelector('.hover-submenu1').firstElementChild.classList.contains('login-img') !== true){
        // hovermenu();
        // }

    // ACCORDION STYLE
    $(document).on("click", '[data-bs-toggle="collapse"]', function () {
        $(this).toggleClass('active').siblings().removeClass('active');
    });

});

	/******* RTL VERSION *******/

	// $('body').addClass('rtl');

    let bodyRtl = $('body').hasClass('rtl');
    if (bodyRtl) {
        $('body').addClass('rtl');
        $("html[lang=en]").attr("dir", "rtl");
        $('body').removeClass('ltr');
        $("head link#style").attr("href", $(this));
        (document.getElementById("style").setAttribute("href", "../assets/plugins/bootstrap/css/bootstrap.rtl.min.css"));
        var carousel = $('.owl-carousel');
        $.each(carousel, function (index, element) {
            // element == this
            var carouselData = $(element).data('owl.carousel');
            carouselData.settings.rtl = true; //don't know if both are necessary
            carouselData.options.rtl = true;
            $(element).trigger('refresh.owl.carousel');
        });
    }
    // RTL STYLE END

 // ***** Horizontal Click Menu ***** //

//  $('body').addClass('horizontal');

let bodyhorizontal = $('body').hasClass('horizontal');
    if (bodyhorizontal) {
        $('body').addClass('horizontal');
        $(".main-content").addClass("hor-content");
        $(".main-content").removeClass("app-content");
        $(".main-container").addClass("container");
        $(".main-container").removeClass("container-fluid");
        $(".main-sidebar").addClass("main-navbar");
        $(".main-sidebar").addClass("sticky");
        $(".main-navbar").removeClass("main-sidebar-sticky")
        $(".main-navbar").removeClass("main-sidebar");
        $(".main-navbar").removeClass("nav");
        $(".main-navbar").removeClass("main-sidebar");
        $(".side-header").addClass("hor-header");
        $(".hor-header").removeClass("side-header");
        $(".hor-header").removeClass("fixed-header");
        $(".app-sidebar").addClass("horizontal-main")
        $(".main-sidebar-body").addClass("container")
        $(".main-sidebar-body").addClass("main-sidemenu")
        $(".main-sidemenu").removeClass("main-sidebar-body")
        $('body').removeClass('sidebar-mini');
        $('body').removeClass('main-sidebar-hide');
        $('body').removeClass('horizontal-hover');
        $('body').removeClass('default-menu');
        $('body').removeClass('icontext-menu');
        $('body').removeClass('icon-overlay');
        $('body').removeClass('closed-menu');
        $('body').removeClass('hover-submenu');
        $('body').removeClass('hover-submenu1');
        $('#slide-left').removeClass('d-none');
        $('#slide-right').removeClass('d-none');
        // $('#slide-left').addClass('d-none');
        // $('#slide-right').addClass('d-none');
        // document.querySelector('.horizontal .nav').style.flexWrap = 'wrap'
        if (!document.querySelector('body').classList.contains('login-img')) {
            if (window.innerWidth >= 992) {
                removeActive()
            }
            else{
            ActiveSubmenu();
            }
            checkHoriMenu();
            responsive();
            document.querySelector('.horizontal .nav.hor-menu').style.flexWrap = 'nowrap'
        }
    }

// HORIZONTAL-HOVER has class
 
//  $('body').addClass('horizontal-hover');

let bodyhorizontalHover = $('body').hasClass('horizontal-hover');
    if (bodyhorizontalHover) {

        $('body').addClass('horizontal-hover');
        $('body').addClass('horizontal');
        $(".main-content").addClass("hor-content");
        $(".main-container").addClass("container");
        $(".main-container").removeClass("container-fluid");
        $(".main-sidebar").addClass("main-navbar");
        $(".main-sidebar").addClass("sticky");
        $(".main-navbar").removeClass("main-sidebar-sticky")
        $(".main-navbar").removeClass("main-sidebar");
        $(".main-navbar").removeClass("nav");
        // $(".main-navbar").removeClass(" ps ");
        $(".side-header").addClass("hor-header");
        $(".hor-header").removeClass("side-header");
        $(".hor-header").removeClass("fixed-header");
        $(".main-sidebar-body").addClass("container")
        $(".main-sidebar-body").addClass("main-sidemenu")
        $(".main-sidemenu").removeClass("main-sidebar-body")
        $('body').removeClass('sidebar-mini');
        $('body').removeClass('main-sidebar-hide');
        $('body').removeClass('default-menu');
        $('body').removeClass('icontext-menu');
        $('body').removeClass('icon-overlay');
        $('body').removeClass('closed-menu');
        $('body').removeClass('hover-submenu');
        $('body').removeClass('hover-submenu1');
        $('#slide-left').removeClass('d-none');
        $('#slide-right').removeClass('d-none');
        // document.querySelector('.horizontal .nav.hor-menu').style.flexWrap = 'nowrap'
        // $('#slide-left').addClass('d-none');
        // $('#slide-right').addClass('d-none');
        // document.querySelector('.horizontal .nav').style.flexWrap = 'wrap'
        if (!document.querySelector('body').classList.contains('login-img')) {
        if (window.innerWidth >= 992) {
            removeActive()
        }
        else{
        ActiveSubmenu();
        }
        checkHoriMenu();
        responsive();
        document.querySelector('.horizontal .nav.hor-menu').style.flexWrap = 'nowrap'
    }
        
    }