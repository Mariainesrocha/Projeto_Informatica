$(document).ready(function () {
    $('ul.navbar-nav li a').click(function (e) {
            $('ul.navbar-nav li a').removeClass('active');
            $(this).addClass('active');
        });

    $(function () {
        $(document).scroll(function () {
            var $nav = $("#my-nav");
            $nav.toggleClass('scrolled', $(this).scrollTop() > $nav.height());
        });
    });

   

    const sectionHome = Array.from(document.querySelectorAll("section")).forEach(el => {
        if(el){
            const observer = new IntersectionObserver((entries) => {
                sectionObserver(entries, observer, el)
            },
            {threshold:0.8}); 
            observer.observe(el);
        } 
    })
        

        const sectionObserver = (entries, sectionObserver) => {
            entries.forEach(entry=>{
                if(entry.isIntersecting) {
                    //remove
                    console.log(entry.target.id);
                    console.log('#'+entry.target.id+'Nav');
                    $('ul.navbar-nav li a').removeClass('active');
                    $('#'+entry.target.id+'Nav').addClass('active');
                }
            });
        }
   

    function makeTimer() {

        //		var endTime = new Date("29 April 2018 9:56:00 GMT+01:00");	
            var endTime = new Date("21 June 2021 23:59:59 GMT+00:00");			
                endTime = (Date.parse(endTime) / 1000);
    
                var now = new Date();
                now = (Date.parse(now) / 1000);
    
                var timeLeft = endTime - now;
    
                var days = Math.floor(timeLeft / 86400);
      
                if (days < "0") { days = "0"; hours="0"}
    
                $("#AboutDaysLeft").html(days);
    
        }
    
    setInterval(function() { makeTimer(); }, 1000);
});


/*
Anchors

#aboutTech
#aboutAbout
#aboutResources
#aboutFAQ
#aboutContact

*/