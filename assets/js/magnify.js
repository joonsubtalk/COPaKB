/**
 *
 * Author URL - https://twitter.com/ruby_on_tails
 * Plugin source tutorial - http://codetheory.in/image-zoom-magnifying-glass-effect-with-css3-and-jquery/
 * Original source tutorial - http://thecodeplayer.com/walkthrough/magnifying-glass-for-images-using-jquery-and-css3
 * Codepen source code - http://codepen.io/scott23/details/akKqc
 */
 
$(document).ready(function(){

	var native_width = 0;
	var native_height = 0;
	$(".large").css("background", "url('" + $(".thumb").attr("src") + "') no-repeat");

	$(".magnify").mousemove(function (e) {
		//When the user hovers on the image, the script will first calculate
		//the native dimensions if they don't exist. Only after the native dimensions
		//are available, the script will show the zoomed version.
		if(!native_width && !native_height)
		{
			//This will create a new image object with the same image as that in .small
			//We cannot directly get the dimensions from .small because of the 
			//width specified to 200px in the html. To get the actual dimensions we have
			//created this image object.
			var image_object = new Image();
			image_object.src = $(".thumb").attr("src");
			
			//This code is wrapped in the .load function which is important.
			//width and height of the object would return 0 if accessed before 
			//the image gets loaded.
			native_width = image_object.width;
			native_height = image_object.height;
		}
		else
		{
			//x/y coordinates of the mouse
			//This is the position of .magnify with respect to the document.
			var magnify_offset = $(this).offset();
			//We will deduct the positions of .magnify from the mouse positions with
			//respect to the document to get the mouse positions with respect to the 
			//container(.magnify)
			var mx = e.pageX - magnify_offset.left;
			var my = e.pageY - magnify_offset.top;
		
			if($(".large").is(":visible"))
			{
				//The background position of .large will be changed according to the position
				//of the mouse over the .small image. So we will get the ratio of the pixel
				//under the mouse pointer with respect to the image and use that to position the 
				//large image inside the magnifying glass
				var rx = Math.round(mx/$(".thumb").width()*native_width - $(".large").width()/2)*-1;
				var ry = Math.round(my/$(".thumb").height()*native_height - $(".large").height()/2)*-1;
				var bgp = rx + "px " + ry + "px";
				
				//Time to move the magnifying glass with the mouse
				var px = mx - $(".large").width()/2;
				var py = my - $(".large").height()/2;
				px = px + 300;
				//Now the glass moves with the mouse
				//The logic is to deduct half of the glass's width and height from the 
				//mouse coordinates to place it with its center at the mouse coordinates
				
				//If you hover on the image now, you should see the magnifying glass in action
				$(".large").css({left: px, top: py, backgroundPosition: bgp});
			}
		}
	})

    //Finally the code to fade out the glass if the mouse is outside the container
	$(".magnify").hover(function () {
	    $('.large').fadeIn(250);
	},
        function () {
            $('.large').fadeOut(250);
        }
    );
});

/**
 *
 * Author URL - https://twitter.com/ruby_on_tails
 * Plugin source tutorial - http://codetheory.in/image-zoom-magnifying-glass-effect-with-css3-and-jquery/
 * Original source tutorial - http://thecodeplayer.com/walkthrough/magnifying-glass-for-images-using-jquery-and-css3
 * Codepen source code - http://codepen.io/scott23/details/akKqc
 */

$(document).ready(function () {

    var native_width = 0;
    var native_height = 0;
    $(".large1").css("background", "url('" + $(".thumb1").attr("src") + "') no-repeat");

    $(".magnify1").mousemove(function (e) {
        //When the user hovers on the image, the script will first calculate
        //the native dimensions if they don't exist. Only after the native dimensions
        //are available, the script will show the zoomed version.
        if (!native_width && !native_height) {
            //This will create a new image object with the same image as that in .small
            //We cannot directly get the dimensions from .small because of the 
            //width specified to 200px in the html. To get the actual dimensions we have
            //created this image object.
            var image_object = new Image();
            image_object.src = $(".thumb1").attr("src");

            //This code is wrapped in the .load function which is important.
            //width and height of the object would return 0 if accessed before 
            //the image gets loaded.
            native_width = image_object.width;
            native_height = image_object.height;
        }
        else {
            //x/y coordinates of the mouse
            //This is the position of .magnify with respect to the document.
            var magnify_offset = $(this).offset();
            //We will deduct the positions of .magnify from the mouse positions with
            //respect to the document to get the mouse positions with respect to the 
            //container(.magnify)
            var mx = e.pageX - magnify_offset.left;
            var my = e.pageY - magnify_offset.top;

            if ($(".large1").is(":visible")) {
                //The background position of .large will be changed according to the position
                //of the mouse over the .small image. So we will get the ratio of the pixel
                //under the mouse pointer with respect to the image and use that to position the 
                //large image inside the magnifying glass
                var rx = Math.round(mx / $(".thumb1").width() * native_width - $(".large1").width() / 2) * -1;
                var ry = Math.round(my / $(".thumb1").height() * native_height - $(".large1").height() / 2) * -1;
                var bgp = rx + "px " + ry + "px";

                //Time to move the magnifying glass with the mouse
                var px = mx - $(".large1").width() / 2;
                var py = my - $(".large1").height() / 2;
                px = px + 300;
                //Now the glass moves with the mouse
                //The logic is to deduct half of the glass's width and height from the 
                //mouse coordinates to place it with its center at the mouse coordinates

                //If you hover on the image now, you should see the magnifying glass in action
                $(".large1").css({ left: px, top: py, backgroundPosition: bgp });
            }
        }
    })

    //Finally the code to fade out the glass if the mouse is outside the container
    $(".magnify1").hover(function () {
        $('.large1').fadeIn(250);
    },
        function () {
            $('.large1').fadeOut(250);
        }
    );
});