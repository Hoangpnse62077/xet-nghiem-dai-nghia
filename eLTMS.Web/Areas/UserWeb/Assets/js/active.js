(function($) {
    "use strict";
    jQuery(document).on('ready',function() {

        //prevent defult
        var prevent_a = $('.search_form button, .faq_filter a');
        prevent_a.on('click', function(event) {
            event.preventDefault();
            $("<div>").append("default " + event.type + " prevented").appendTo("#log");
        });

        //header_search_bar
        $('.search_form button').on('click', function() {
            $(' form.search_form').toggleClass('active');
        });


        //Radio CHecked content
        $('input').on("click", function () {
            $('.payment_method_area input[type="radio"]:not(:checked)').parent().parent().removeClass("active");
            $('.payment_method_area input[type="radio"]:checked').parent().parent().addClass("active");
        });    

        //Mobile menu plugin 
        if ($.fn.slicknav) {
            $('.main_menu').slicknav({
                prependTo: '.menu_bar',
                label: ''
            });
        };
        //faq menu
        $(window).on('scroll',function() {
            var autoHideFilter = $('.auto-hide-filter');
            if ($(this).scrollTop() > 400) {
                autoHideFilter.addClass('fixed');
            } else {
                autoHideFilter.removeClass('fixed');
            }
        });
        if ($.fn.onePageNav) {
            $('.one_page').onePageNav({
                currentClass: 'active',
                scrollSpeed: 750,
                easing: 'swing'
            });
        }
        //slider plugin 
        if ($.fn.owlCarousel) {
            //==welcome_area==\\
            var wSlider = $('.welcome_slider');
            wSlider.owlCarousel({
                items: 1,
                loop: true,
                smartSpeed: 500,
                autoplay: false,
                mouseDrag: false,
                touchDrag: false,
                dots: true,
                nav: true,
                navText: ['<i class="zmdi zmdi-chevron-left"></i>', '<i class="zmdi zmdi-chevron-right"></i>']
            });

            //==welcome_area animation==\\
            wSlider.on('translate.owl.carousel', function() {
                $('.welcome_content h1').removeClass('fadeInLeft animated').hide();
                $('.welcome_content p').removeClass('fadeInLeft animated').hide();
                $('.welcome_content a').removeClass('fadeInLeft animated').hide();
                $('.welcome_area.v3 img').removeClass('fadeInRight animated').hide();
                $('.w_man').removeClass('fadeInRight animated').hide();
                $('.welcome_slider form').removeClass('fadeInRight animated').hide();
            });
            wSlider.on('translated.owl.carousel', function() {
                $('.owl-item.active .welcome_content h1').addClass('fadeInLeft animated').show();
                $('.owl-item.active .welcome_content p').addClass('fadeInLeft animated').show();
                $('.owl-item.active .welcome_content a').addClass('fadeInLeft animated').show();
                $('.welcome_area.v3 .owl-item.active img').addClass('fadeInRight animated').show();
                $('.owl-item.active .w_man').addClass('fadeInRight animated').show();
                $('.welcome_slider .owl-item.active form').addClass('fadeInRight animated').show();
            });

            //special_food_slider 
            $('.about_slide').owlCarousel({
                items: 1,
                loop: true,
                margin: 0,
                dot: true,
            });

            // Testimonial SLide 
            $('.primary_testimonial_slider').owlCarousel({
                loop: true,
                dot: true,
                autoplay: true,
                center:true,
                navText: ['<i class="zmdi zmdi-long-arrow-left"></i>', '<i class="zmdi zmdi-long-arrow-right"></i>'],
                autoplaySpeed: 1000,
                responsive : {
                    0 : {
                        items: 1
                    },
                    768 : {
                        items:2
                    },
                    992 : {
                        items:3
                    }
                }
            });

            // Testimonial SLide 
           $('.v5_testimonial_slider').owlCarousel({
                loop: true,
                dot: true,
                autoplay: true,
                dots: false,
                nav: true,
                navText: ['<i class="zmdi zmdi-long-arrow-left"></i>', '<i class="zmdi zmdi-long-arrow-right"></i>'],
                autoplaySpeed: 1000,
                responsive : {
                    0 : {
                        items: 1
                    },
                    768 : {
                        items:2
                    }
                }
            });

            // Testimonial SLide 
            $('.eye_shop_slider').owlCarousel({
                loop: true,
                dots: false,
                autoplay: true,
                nav:true,
                navText: ['<i class="zmdi zmdi-long-arrow-left"></i>', '<i class="zmdi zmdi-long-arrow-right"></i>'],
                margin: 30,
                autoplaySpeed: 1000,
                responsive : {
                    0 : {
                        items: 1
                    },
                    768 : {
                        items:2
                    },
                    992 : {
                        items:4
                    }
                }
            });
            // Testimonial SLide 
            $('.doctor_slider').owlCarousel({
                loop: true,
                dots: false,
                autoplay: true,
                smartSpeed: 500,
                nav: true,
                margin:30,
                autoplaySpeed: 1000,
                center: true,
                navText: ['<i class="zmdi zmdi-chevron-left"></i>', '<i class="zmdi zmdi-chevron-right"></i>'],
                responsive : {
                    0 : {
                        items: 1
                    },
                    768 : {
                        items:2
                    },
                    992 : {
                        items:3
                    }
                }
            });
            $('.v4_service_slider').owlCarousel({
                loop: true,
                dots: false,
                autoplay: true,
                smartSpeed: 500,
                nav: true,
                autoplaySpeed: 1000,
                navText: ['<i class="zmdi zmdi-long-arrow-left"></i>', '<i class="zmdi zmdi-long-arrow-right"></i>'],
                responsive : {
                    0 : {
                        items: 1
                    },
                    768 : {
                        items:2
                    },
                    992 : {
                        items:3
                    }
                }
            });
        };
        if ($.fn.counterUp) {
            $('.count').counterUp({
                delay: 100,
                time: 2000
            });
        }

        //Filter Menu 
        if ($.fn.isotope) {
            var istoopeLi = $('.isotope_menu li');
            istoopeLi.on("click", function() {
                istoopeLi.removeClass("active");
                $(this).addClass("active");
                var selector = $(this).attr('data-name');
                $(".isotope").isotope({
                    filter: selector,
                    animationOptions: {
                        duration: 750,
                        easing: 'linear',
                        queue: false
                    }
                });
                return false;
            });
        };

        //Image popup Plugin 
        if ($.fn.magnificPopup) {
            $('.img_popup').magnificPopup({
                type: 'image',
                mainClass: 'mfp-with-zoom', // this class is for CSS animation below
                zoom: {
                    enabled: true, // By default it's false, so don't forget to enable it
                    duration: 300, // duration of the effect, in milliseconds
                    easing: 'ease-in-out', // CSS transition easing function
                    // The "opener" function should return the element from which popup will be zoomed in
                    // and to which popup will be scaled down
                    // By defailt it looks for an image tag:
                    opener: function(openerElement) {
                        // openerElement is the element on which popup was initialized, in this case its <a> tag
                        // you don't need to add "opener" option if this code matches your needs, it's defailt one.
                        return openerElement.is('a') ? openerElement : openerElement.find('a');
                    }
                }
            });
            $('.video_popup').magnificPopup({
                type: 'iframe',
            });
        };

        //Select form plugin
        if ($.fn.niceSelect) {
            $('select').niceSelect();
        };

        //Chef double slider
        if ($.fn.slick) {
            $('.v4_testimonial_2').slick({
                slidesToShow: 1,
                slidesToScroll: 1,
                arrows: false,
                asNavFor: '.v4_testimonial_1',
                infinite: true
            });

            $('.v4_testimonial_1').slick({
                slidesToShow: 3,
                slidesToScroll: 1,
                centerMode: true,
                asNavFor: '.v4_testimonial_2',
                focusOnSelect: true,
                arrows: true,
                prevArrow: '<i class="left-arrow zmdi zmdi-long-arrow-left"></i>',
                nextArrow: '<i class="right-arrow zmdi zmdi-long-arrow-right"></i>',
                infinite: true,
            });
        };


        if($.fn.fullCalendar){
            initThemeChooser({
                init: function(themeSystem) {
                    $('#calendar').fullCalendar({
                        themeSystem: themeSystem,
                        header: {
                            left: 'prev,next today',
                            center: 'title',
                            right: 'month,agendaWeek,agendaDay,listWeek'

                        },
                        defaultDate: '2017-10-12',
                        weekNumbers: true,
                        navLinks: true, // can click day/week names to navigate views
                        editable: true,
                        eventLimit: true, // allow "more" link when too many events
                        events: [{
                                title: 'All Day Event',
                                start: '2017-10-01'
                            },
                            {
                                title: 'Long Event',
                                start: '2017-10-07',
                                end: '2017-10-10',
                            },
                            {
                                id: 999,
                                title: 'Repeating Event',
                                start: '2017-10-09T16:00:00'
                            },
                            {
                                id: 999,
                                title: 'Repeating Event',
                                start: '2017-10-16T16:00:00'
                            },
                            {
                                title: 'Conference',
                                start: '2017-10-11',
                                end: '2017-10-13'
                            },
                            {
                                title: 'Meeting',
                                start: '2017-10-12T10:30:00',
                                end: '2017-10-12T12:30:00'
                            },
                            {
                                title: 'Lunch',
                                start: '2017-10-12T12:00:00'
                            },
                            {
                                title: 'Meeting',
                                start: '2017-10-12T14:30:00'
                            },
                            {
                                title: 'Happy Hour',
                                start: '2017-10-12T17:30:00'
                            },
                            {
                                title: 'Dinner',
                                start: '2017-10-12T20:00:00'
                            },
                            {
                                title: 'Birthday Party',
                                start: '2017-10-13T07:00:00'
                            },
                            {
                                title: 'Click for Google',
                                url: 'http://google.com/',
                                start: '2017-10-28'
                            }
                        ]
                    });
                },

                change: function(themeSystem) {
                    $('#calendar').fullCalendar('option', 'themeSystem', themeSystem);
                }

            });
        }


    });

    jQuery(window).on("load", function() {

        //Active isotope in container
        if ($.fn.isotope) {
            $(".isotope").isotope({
                itemSelector: '.isotope_wraper',
                layoutMode: 'fitRows'
            });
            $(".tc_masonry").isotope({
                itemSelector: '.single_tc_img',
                  masonry: {
                    // use element for option
                    columnWidth: '.single_tc_img'
                  }
            });
        };
        // Slider Function
        var sync1 = $("#sync1");
        var sync2 = $("#sync2");
        var slidesPerPage = 3; //globaly define number of elements per page
        var syncedSecondary = true;

        sync1.owlCarousel({
            items: 1,
            slideSpeed: 2000,
            autoplay: true,
            dots: true,
            loop: true,
            responsiveRefreshRate: 200,
        }).on('changed.owl.carousel', syncPosition);

        sync2.on('initialized.owl.carousel', function() {
                sync2.find(".owl-item").eq(0).addClass("current");
            })
            .owlCarousel({
                items: slidesPerPage,
                dots: false,
                smartSpeed: 200,
                slideSpeed: 500,
                slideBy: slidesPerPage, //alternatively you can slide by 1, this way the active slide will stick to the first item in the second carousel
                responsiveRefreshRate: 100
            }).on('changed.owl.carousel', syncPosition2);

        function syncPosition(el) {
            //if you set loop to false, you have to restore this next line
            //var current = el.item.index;

            //if you disable loop you have to comment this block
            var count = el.item.count - 1;
            var current = Math.round(el.item.index - (el.item.count / 2) - .5);

            if (current < 0) {
                current = count;
            }
            if (current > count)  {
                current = 0;
            }

            //end block

            sync2
                .find(".owl-item")
                .removeClass("current")
                .eq(current)
                .addClass("current");
            var onscreen = sync2.find('.owl-item.active').length - 1;
            var start = sync2.find('.owl-item.active').first().index();
            var end = sync2.find('.owl-item.active').last().index();

            if (current > end) {
                sync2.data('owl.carousel').to(current, 100, true);
            }
            if (current < start) {
                sync2.data('owl.carousel').to(current - onscreen, 100, true);
            }
        }

        function syncPosition2(el) {
            if (syncedSecondary) {
                var number = el.item.index;
                sync1.data('owl.carousel').to(number, 100, true);
            }
        }

        sync2.on("click", ".owl-item", function(e) {
            e.preventDefault();
            var number = $(this).index();
            sync1.data('owl.carousel').to(number, 300, true);
        });

        //Custom Preloader
        $('.preloader').fadeOut(300);
    });
})(jQuery);



