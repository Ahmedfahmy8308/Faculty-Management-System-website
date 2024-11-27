(function () {
  "use strict";

  /*  Easy selector helper function */
  const select = (el, all = false) => {
    el = el.trim()
    if (all) {
      return [...document.querySelectorAll(el)]
    } else {
      return document.querySelector(el)
    }
  }

  /**
   * Easy event listener function
   */
  const on = (type, el, listener, all = false) => {
    let selectEl = select(el, all)
    if (selectEl) {
      if (all) {
        selectEl.forEach(e => e.addEventListener(type, listener))
      } else {
        selectEl.addEventListener(type, listener)
      }
    }
  }

  /**
   * Easy on scroll event listener 
   */
  const onscroll = (el, listener) => {
    el.addEventListener('scroll', listener)
  }

  /* Navbar links active state on scroll */

    $(document).ready(function () {
        var $navbarlinks = $('#navbar .scrollto');

        function navbarlinksActive() {
            var position = $(window).scrollTop() + 200;
            $navbarlinks.each(function () {
                var $navbarlink = $(this);
                if (!$navbarlink.attr('href')) return;
                var $section = $($navbarlink.attr('href'));
                if (!$section.length) return;
                if (position >= $section.offset().top && position <= ($section.offset().top + $section.outerHeight())) {
                    $navbarlink.addClass('active');
                } else {
                    $navbarlink.removeClass('active');
                }
            });
        }

        $(window).on('load', navbarlinksActive);
        $(document).on('scroll', navbarlinksActive);
    });


  /*  Scrolls to an element with header offset */
    function scrollto(el) {
        var header = $('#header');
        var offset = header.outerHeight();

        var elementPos = $(el).offset().top;
        $('html, body').animate({
            scrollTop: elementPos - offset
        }, 'slow');
    }


  /* scrolled header  */
  let selectHeader = select('#header')
  if (selectHeader) {
    const headerScrolled = () => {
      if (window.scrollY > 100) {
        selectHeader.classList.add('header-scrolled')
      } else {
        selectHeader.classList.remove('header-scrolled')
      }
    }
    window.addEventListener('load', headerScrolled)
    onscroll(document, headerScrolled)
  }

  /**
   * Back to top button
   */
  let backtotop = select('.back-to-top')
  if (backtotop) {
    const toggleBacktotop = () => {
      if (window.scrollY > 100) {
        backtotop.classList.add('active')
      } else {
        backtotop.classList.remove('active')
      }
    }
    window.addEventListener('load', toggleBacktotop)
    onscroll(document, toggleBacktotop)
  }

  /**
   * Mobile nav toggle
   */
  on('click', '.mobile-nav-toggle', function (e) {
    select('#navbar').classList.toggle('navbar-mobile')
    this.classList.toggle('bi-list')
    this.classList.toggle('bi-x')
  })

  /**
   * Scrool with ofset on links with a class name .scrollto
   */
  on('click', '.scrollto', function (e) {
    if (select(this.hash)) {
      e.preventDefault()

      let navbar = select('#navbar')
      if (navbar.classList.contains('navbar-mobile')) {
        navbar.classList.remove('navbar-mobile')
        let navbarToggle = select('.mobile-nav-toggle')
        navbarToggle.classList.toggle('bi-list')
        navbarToggle.classList.toggle('bi-x')
      }
      scrollto(this.hash)
    }
  }, true)

  /**
   * Scroll with ofset on page load with hash links in the url
   */
  window.addEventListener('load', () => {
    if (window.location.hash) {
      if (select(window.location.hash)) {
        scrollto(window.location.hash)
      }
    }
  });

  /**
   * Preloader
   */
  let preloader = select('#preloader');
  if (preloader) {
    window.addEventListener('load', () => {
      preloader.remove()
    });
  }

  /**
   * Initiate  glightbox 
   */
  const glightbox = GLightbox({
    selector: '.glightbox'
  });


  /**
   * Animation on scroll
   */
  window.addEventListener('load', () => {
    AOS.init({
      duration: 1000,
      easing: "ease-in-out",
      once: true,
      mirror: false
    });
  });

})()


/*Dark theme converting*/
$(document).ready(function () {
  $('#icon').click(function () {
    $('body').toggleClass('dark-theme');
    if ($('body').hasClass('dark-theme')) {
      $('#icon').attr('src', 'assets/img/sun.png');
    } else {
      $('#icon').attr('src', 'assets/img/moon.png');
    }
  });
});


/* log in button*/
$(document).ready(function () {
  $('#student-button').click(function () {
    window.location.href = 'stu-home.html';
  });
  $('#doctor-button').click(function () {
    window.location.href = 'doc-home.html';
  }); $('#affairs-button').click(function () {
    window.location.href = 'Affairs-home.html';
  });
  $('#result-button').click(function () {
    window.location.href = 'result.html';
  });
});



$(document).ready(function () {
  $('#toggle-password').click(function () {
    var passwordInput = $('#password');

    if (passwordInput.attr('type') === 'password') {
      passwordInput.attr('type', 'text');
      $(this).removeClass('fa-eye').addClass('fa-eye-slash');
    } else {
      passwordInput.attr('type', 'password');
      $(this).removeClass('fa-eye-slash').addClass('fa-eye');
    }
  });
  $('#toggle-password2').click(function () {
    var passwordInput = $('#password');

    if (passwordInput.attr('type') === 'password') {
      passwordInput.attr('type', 'text');
      $(this).removeClass('fa-eye').addClass('fa-eye-slash');
    } else {
      passwordInput.attr('type', 'password');
      $(this).removeClass('fa-eye-slash').addClass('fa-eye');
    }
  });
});
