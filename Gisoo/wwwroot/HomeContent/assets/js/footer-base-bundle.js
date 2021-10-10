$(document).ready(()=> {

    $('a[href="#"]').click(function(e){
        e.preventDefault();
    });


    $('[data-toggle]').on('click', function()
    {
        let toggle_name = $(this).data('toggle');
        let target_name = $(this).data('target');
        let target_element = $('[data-toggle="showbox"][data-target="'+target_name+'"]');

        if(toggle_name == 'openbox')
        {
            let target_status = target_element.data('status');
            $('[data-toggle="showbox"]:not([data-target="'+target_name+'"])').attr('data-status',false);

            if(target_status == false)
            {
                target_element.attr('data-status',true);
            }
            else
            {
                target_element.attr('data-status',false);
            }
        }
        else if(toggle_name == 'closebox')
        {
            target_element.attr('data-status',false);
        }
    });

    $(document).on('click',function(){
        $('[data-toggle="showbox"]').attr('data-status',false);
    });

    $(document).on('keyup',function(e) {
        if (e.key === "Escape") {
            $('[data-toggle="showbox"]').attr('data-status',false);
        }
    });

    $('[data-toggle="showbox"],[data-toggle="openbox"]').click(function(event){
        event.stopPropagation();
    });



    // Tabs
    //=====================================================================

    $(document).on('click','li[data-tab]',function(){
        let $this = $(this),
            tab = $this.data('tab'),
            target = $this.data('tab-target');

        $this.addClass('active')
            .siblings()
            .removeClass('active');

        $('.tab-content[data-tab-target="'+target+'"][data-tab="'+tab+'"]')
            .addClass('active')
            .siblings()
            .removeClass('active');
    });


    $(document).on('mouseover','li[data-tab-hover]',function(){
        let $this = $(this),
            tab = $this.data('tab-hover'),
            target = $this.data('tab-target');

        $this.addClass('active')
            .siblings()
            .removeClass('active');

        $('.tab-content[data-tab-target="'+target+'"][data-tab="'+tab+'"]')
            .addClass('active')
            .siblings()
            .removeClass('active');
    });


    // Toaster Alert
    //=====================================================================

    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        onOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    var scroll = $(window).scrollTop();
    if(scroll > 50)
    {
        $('body').addClass('body--header-fix')
    }
    else
    {
        $('body').removeClass('body--header-fix')
    }

    $(window).scroll(()=> {
        scroll = $(window).scrollTop();
        if(scroll > 50)
        {
            $('body').addClass('body--header-fix')
        }
        else
        {
            $('body').removeClass('body--header-fix')
        }
    });

    // Toast.fire({
    //     icon: 'success',
    //     title: 'توستر',
    //     footer: 'این تست توستر است',
    // });

    $('.profile__sidebar nav li').has('>ul').addClass('has-child').prepend('<i class="dn-arrow-down"></i>');

    $('.profile__sidebar nav li.has-child >a').on('click',function(){

        let $this = $(this).parent();
        let $child = $(this).siblings('ul');
        if($this.hasClass('active'))
        {
            $child.slideUp(200,function(){
                $this.removeClass('active');
            });
        }
        else
        {
            $child.slideDown(200,function(){
                $this.addClass('active');
            });
        }

    });


    //$('#profile__header1').on('click', function () {
    //    //e.preventDefault();
    //    //e.stopPropagation();
    //    $('.profile__wrapper').toggleClass('profile__wrapper--collapse');
        
    //});

    


    $(".datepicker").persianDatepicker({
        observer: true,
        format: 'YYYY/MM/DD',
        autoClose: true
    });

    

    $(document).on('click','.faq__item',function(){
        var $this = $(this);
        var $content = $this.find('.faq__item__content');
        $content.slideDown(500,function (){
            $this.addClass('faq__item--active').siblings().find('.faq__item__content').slideUp(500,function (){
                $this.siblings().removeClass('faq__item--active')
            })
        })
    });


    $(document).on('keyup','.profile__content__wallets__search input',function(){
        var val = $(this).val();
        $(".profile__content__wallets__item").each(function() {
            if (
                ($(this).data('en-title').search(val) == 0) || 
                ($(this).data('fa-title').search(val) == 0) || 
                ($(this).data('min-title').search(val) == 0)
                )
            {
                $(this).removeClass('profile__content__wallets__item--no-result');
            }
            else {
                $(this).addClass('profile__content__wallets__item--no-result');
            }
        });

    });


 
    if($('#ads_chart').length > 0)
    {
        Chart.defaults.global.defaultFontFamily = 'yekan';

        var ads_chart = new Chart($('#ads_chart')[0], {
            type: 'line',
            data: {
                datasets: [{
                    data: [5,10,3,8,5,10,8,7,2,9],
                    // data: [0,0,0,0,0,0,0,0,0,0,0,0],
                    backgroundColor: '#f2a900',
                    borderColor: '#999',
                    fill: false,
                    label: 'میزان بازدید'
                }],
                labels: [
                    '1399/10/01',
                    '1399/10/02',
                    '1399/10/03',
                    '1399/10/04',
                    '1399/10/05',
                    '1399/10/06',
                    '1399/10/07',
                    '1399/10/08',
                    '1399/10/09',
                    '1399/10/10'
                ]
            },
            options:
            {
                elements:
                {
                    line:
                    {
                        tension: 0.000001,
                    }
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero:true,
                            stepSize: 1
                        }
                    }]
                },
                maintainAspectRatio: false,
            }
        });
    }
    


    





    var main_slider = new Swiper('.intro .swiper-container', {
        slidesPerView: 1,
        spaceBetween: 30,
        // loop: 1,
        speed: 1500,
        navigation: {
            nextEl: '.swiper-btn-next',
            prevEl: '.swiper-btn-prev',
        },
        autoplay: {
            delay: 3000,
            disableOnInteraction: 0
        }
    });

    var ads_slider = new Swiper('.ads-slider__wrapper .swiper-container', {
        slidesPerView:1.2,
        spaceBetween: 20,
        // loop: 1,
        speed: 300,
        autoplay: {
            delay: 3000,
            disableOnInteraction: 0
        },
        breakpoints:
        {
            1200:{
                slidesPerView: 4,
            },
            992: {
                slidesPerView: 3,
            },
            767: {
                slidesPerView: 2,
            },
            480: {
                slidesPerView: 1.2,
            },
        },
        navigation: {
            nextEl: '.swiper-btn-next',
            prevEl: '.swiper-btn-prev',
        },
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
        },
    });
    
    $(document).on('mouseover','.ads-slider__wrapper',function(){
        if(ads_slider.length > 1)
        {
            $.each(ads_slider,function(i,e){
                ads_slider[i].autoplay.stop();
            })
        }
        else
        {
            ads_slider.autoplay.stop();
        }
    })
    $(document).on('mouseleave','.ads-slider__wrapper',function(){
        if(ads_slider.length > 1)
        {
            $.each(ads_slider,function(i,e){
                ads_slider[i].autoplay.start();
            })
        }
        else
        {
            ads_slider.autoplay.start();
        }
    })
    

    var salon_slider = new Swiper('.salon-slider__wrapper .swiper-container', {
        slidesPerView:1.2,
        spaceBetween: 20,
        // loop: 1,
        speed: 300,
        autoplay: {
            delay: 3000,
            disableOnInteraction: 0
        },
        breakpoints: {
            1200: {
                slidesPerView:4,
            },
            992: {
                slidesPerView: 3,
            },
            767: {
                slidesPerView:2,
            },
            480: {
                slidesPerView:1.2,
            },
        },
        navigation: {
            nextEl: '.swiper-btn-next',
            prevEl: '.swiper-btn-prev',
        },
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
        },
    });

    $(document).on('mouseover','.salon-slider__wrapper',function(){
        if(salon_slider.length > 1)
        {
            $.each(salon_slider,function(i,e){
                salon_slider[i].autoplay.stop();
            })
        }
        else
        {
            salon_slider.autoplay.stop();
        }
    })
    $(document).on('mouseleave','.salon-slider__wrapper',function(){
        if(salon_slider.length > 1)
        {
            $.each(salon_slider,function(i,e){
                salon_slider[i].autoplay.start();
            })
        }
        else
        {
            salon_slider.autoplay.start();
        }
    })

    var learn_slider = new Swiper('.learn-slider__wrapper .swiper-container', {
        slidesPerView: 1.2,
        spaceBetween: 20,
        // loop: 1,
        speed: 300,
        autoplay: {
            delay: 3000,
            disableOnInteraction: 0
        },
        breakpoints: {
            1200: {
                slidesPerView:4,
            },
            992: {
                slidesPerView: 3,
            },
            767: {
                slidesPerView:2,
            },
            480: {
                slidesPerView:1.2,
            },
        },
        navigation: {
            nextEl: '.swiper-btn-next',
            prevEl: '.swiper-btn-prev',
        },
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
        },
    });

    $(document).on('mouseover','.learn-slider__wrapper',function(){
        if(learn_slider.length > 1)
        {
            $.each(learn_slider,function(i,e){
                learn_slider[i].autoplay.stop();
            })
        }
        else
        {
            learn_slider.autoplay.stop();
        }
    })
    $(document).on('mouseleave','.learn-slider__wrapper',function(){
        if(learn_slider.length > 1)
        {
            $.each(learn_slider,function(i,e){
                learn_slider[i].autoplay.start();
            })
        }
        else
        {
            learn_slider.autoplay.start();
        }
    })

    var about_slider = new Swiper('.about__slider .swiper-container', {
        slidesPerView: 1,
        spaceBetween: 15,
        loop: 1,
        speed: 300,
        autoplay: {
            delay: 2000,
            disableOnInteraction: 0
        },
    });

    var product_slider = new Swiper('.product-slider__wrapper .swiper-container', {
        slidesPerView: 1.2,
        spaceBetween: 20,
        // loop: 1,
        speed: 300,
        autoplay: {
            delay: 3000,
            disableOnInteraction: 0
        },
        breakpoints: {
            1200: {
                slidesPerView:4,
            },
            992: {
                slidesPerView: 3,
            },
            767: {
                slidesPerView:2,
            },
            480: {
                slidesPerView:1.2,
            },
        },
        navigation: {
            nextEl: '.swiper-btn-next',
            prevEl: '.swiper-btn-prev',
        },
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
        },
    });

    $(document).on('mouseover','.product-slider__wrapper',function(){
        if(product_slider.length > 1)
        {
            $.each(product_slider,function(i,e){
                product_slider[i].autoplay.stop();
            })
        }
        else
        {
            product_slider.autoplay.stop();
        }
    })
    $(document).on('mouseleave','.product-slider__wrapper',function(){
    	if(product_slider.length > 1)
        {
            $.each(product_slider,function(i,e){
                product_slider[i].autoplay.start();
            })
        }
        else
        {
            product_slider.autoplay.start();
        }
    }) 


    var salon_content_slider = new Swiper('.salon-content__slider .swiper-container', {
        slidesPerView: 1,
        spaceBetween: 30,
        // loop: 1,
        speed: 300,
        autoplay: {
            delay: 3000,
            disableOnInteraction: 0
        },
        navigation: {
            nextEl: '.swiper-btn-next',
            prevEl: '.swiper-btn-prev',
        },
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
        },
    });

    var ads_content_slider = new Swiper('.ads-content__slider .swiper-container', {
        slidesPerView: 1,
        spaceBetween: 30,
        // loop: 1,
        speed: 300,
        autoplay: {
            delay: 3000,
            disableOnInteraction: 0
        },
        navigation: {
            nextEl: '.swiper-btn-next',
            prevEl: '.swiper-btn-prev',
        },
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
        },
    });


    if($('#mapid').length > 0)
    {
        var mymap = L.map('mapid').setView([32.6538472, 51.6724925], 13);
        L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw', {
            attribution: '',
            maxZoom: 18,
            id: 'mapbox/streets-v11',
            tileSize: 512,
            zoomOffset: -1,
            accessToken: 'your.mapbox.access.token'
        }).addTo(mymap);

        var map_icon = L.icon({
            iconUrl: './assets/img/marker.png',
            iconSize: [25, 33],
            iconAnchor: [25, 33],
            popupAnchor: [0, -33]
        });

        L.marker([32.6538472, 51.6724925],{icon:map_icon,}).addTo(mymap);
    }
    


    var opt = {
        autoWidth: false,
        responsive: true,
        dom: '<"data-table__header"lf>rt<"data-table__footer"ip>',
        lengthMenu: [[10, 25, 50, 100, 200, -1], [10, 25, 50, 100, 200, "همه"]],
        order: [],
        columnDefs: [{
            targets: "_all",
            orderable: true,
            type: 'pstring'
        }],
        language: {
            "decimal": "",
            "emptyTable": "نتیجهی یافت نشد",
            "info": "نمایش _START_ تا _END_ از _TOTAL_ نتیجه",
            "infoEmpty": "نمایش 0 تا 0 از 0 نتیجه",
            "infoFiltered": "(فیلتر شده از _MAX_ نتیجه)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": " _MENU_",
            "loadingRecords": "در حال بارگذاری...",
            "processing": "در حال پردازش...",
            "search": "",
            "searchPlaceholder": "جستجو ...",
            "zeroRecords": "نتیجه ای پیدا نشد",
            "paginate": {
                "first": "اول",
                "last": "آخر",
                "next": "بعدی",
                "previous": "قبلی"
            },
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            }
        }
    };
    
    var data_table = $('.data-table').DataTable(opt);

    
    //=============================//
    // r.dadkhah.tehrani@gmail.com //
    //=============================//
});

//# sourceMappingURL=footer-base-bundle.js.map
//var
//    persianNumbers = [/۰/g, /۱/g, /۲/g, /۳/g, /۴/g, /۵/g, /۶/g, /۷/g, /۸/g, /۹/g],
//    arabicNumbers = [/٠/g, /١/g, /٢/g, /٣/g, /٤/g, /٥/g, /٦/g, /٧/g, /٨/g, /٩/g],
//    fixNumbers = function (str) {
//        if (typeof str === 'string') {
//            for (var i = 0; i < 10; i++) {
//                str = str.replace(persianNumbers[i], i).replace(arabicNumbers[i], i);
//            }
//        }
//        return str;
//    };
//$('input').keyup(function () {
//    $(this).val(fixNumbers($(this).val()));
//})
