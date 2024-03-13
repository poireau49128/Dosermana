const swiper_main = new Swiper('.swiper-main', {
    direction: 'horizontal',
    /*loop: true,*/
    rewind: true,
    autoplay: {
        delay: 5000,
        disableOnInteraction: false,
    },
    scrollbar: {
        el: '.swiper-scrollbar',
        draggable: true,
    },
});