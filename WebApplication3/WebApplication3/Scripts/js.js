$(document).ready(function () {

    var perHub = $.connection.perHub;
    $.connection.hub.logging = true;
    $.connection.hub.start();
    //perHub.client.newMessage = function (message) {
    //    console.log("mes= " + message);
    //};
    var on_off = $("#on_off");
    var s4 = $("#s4");
    var s3 = $("#s3");
    var s2 = $("#s2");
    var s1 = $("#s1");

    var m1 = $("#m1");
    var m2 = $("#m2");
    var m3 = $("#m3");
    var m4 = $("#m4");
    var m4 = $("#m4");

    var b1 = $("#b1");
    var b2 = $("#b2");
    var b3 = $("#b3");
    var b4 = $("#b4");

    var alarma = $("#alarma");

    on_off.click(function () {
            perHub.server.send("on_off", 0);
    });
    s1.click(function () {
        perHub.server.send("s", 1);
    });

    s2.click(function () {        
        perHub.server.send("s", 2);
    });

    s3.click(function () {
        perHub.server.send("s", 3);
    });

    s4.click(function () {
        perHub.server.send("s", 4);
    });

    m1.click(function () {
        perHub.server.send("m", 1);
    });

    m2.click(function () {
        perHub.server.send("m", 2);
    });

    m3.click(function () {
        perHub.server.send("m", 3);
    });

    m4.click(function () {
        perHub.server.send("m", 4);
    });




    perHub.client.o0 = function (message) {
        console.log("o0= " + message);
        var i = 0;

        for (i = 0; i < 8; i++) {
            var mask = 1 << i;
            if ((message & mask) != 0) {
                // bit is set
                switch (i) {
                    case 0: {

                    } break;
                    case 1: { // pompa 1 ??  q0.1
                        s1.css({
                            "background-color": "green"
                        });

                        m1.css({
                            "background-color": "green"
                        });
                    } break;
                    case 2: { // pompa 2 ?? q0.2
                        s2.css({
                            "background-color": "green"
                        });
                        m2.css({
                            "background-color": "green"
                        });
                    } break;
                    case 3: { // pompa 3 ??
                        s3.css({
                            "background-color": "green"
                        });
                        m3.css({
                            "background-color": "green"
                        });
                    } break;
                    case 4: {  // pompa 4 ??
                        s4.css({
                            "background-color": "green"
                        });
                        m4.css({
                            "background-color": "green"
                        });
                    } break;
                    case 5: {
                        alarma.css({
                            "background-color": "red"
                        });
                    } break;
                    case 6: {

                    } break;
                    case 7: {

                    } break;

                }

            } else {
                // bit is not set
                switch (i) {
                    case 0: {

                    } break;
                    case 1: {
                        s1.css({
                            "background-color": "black"
                        });
                        m1.css({
                            "background-color": "transparent"
                        });
                    } break;
                    case 2: {
                        s2.css({
                            "background-color": "black"
                        });

                        m2.css({
                            "background-color": "transparent"
                        });
                    } break;
                    case 3: {
                        s3.css({
                            "background-color": "black"
                        });
                        m3.css({
                            "background-color": "transparent"
                        });
                    } break;
                    case 4: {
                        m4.css({
                            "background-color": "transparent"
                        });
                        s4.css({
                            "background-color": "black"
                        });
                    } break;
                    case 5: {
                        alarma.css({
                            "background-color": "transparent"
                        });
                    } break;
                    case 6: {

                    } break;
                    case 7: {

                    } break;

                }
            }

        }


    };
    perHub.client.i0 = function (message) {
        console.log("i0= " + message);
        var i = 0;

        for (i = 0; i < 8; i++) {
            var mask = 1 << i;
            if ((message & mask) != 0) {
                // bit is set
                switch (i) {
                    case 0: {
                        s0.css({
                            "border": "1px"
                        });
                    } break;
                    case 1: {
                        console.log("s1 apasat");
                        s1.css({
                            "border": "1px"
                        });
                    } break;
                    case 2: {
                        s2.css({
                            "border": "1px"
                        });
                    } break;
                    case 3: {
                        s3.css({
                            "border": "1px"
                        });
                    } break;
                    case 4: {
                        s4.css({
                            "border": "1px"
                        });
                    } break;
                    case 5: {
                    } break;
                    case 6: {
                        b1.css({
                            "background-color": "red"
                        });
                    } break;
                    case 7: {
                        b2.css({
                            "background-color": "green"
                        });
                    } break;

                }

            } else {
                // bit is not set
                switch (i) {
                    case 0: {
                        s0.css({
                            "border": "4px"
                        });
                    } break;
                    case 1: {
                        console.log("s1 apasat");
                        s1.css({
                            "border": "4px"
                        });
                    } break;
                    case 2: {
                        s2.css({
                            "border": "4px"
                        });
                    } break;
                    case 3: {
                        s3.css({
                            "border": "4px"
                        });
                    } break;
                    case 4: {
                        s4.css({
                            "border": "4px"
                        });
                    } break;
                    case 5: {
                    } break;
                    case 6: { // senzor b1 ??
                        b1.css({
                            "background-color": "transparent"
                        });
                    } break;
                    case 7: {
                        b2.css({
                            "background-color": "transparent"
                        });
                    } break;

                }
            }

        }

    };

    perHub.client.o8 = function (message) {
        console.log("o8= " + message);


    };
    perHub.client.i8 = function (message) {
        console.log("i8= " + message);
        var i = 0;

        for (i = 0; i < 8; i++) {
            var mask = 1 << i;
            if ((message & mask) != 0) {
                // bit is set
                switch (i) {
                    case 0: {
                        b3.css({
                            "background-color": "red"
                        });
                    } break;
                    case 1: {
                        b4.css({
                            "background-color": "red"
                        });
                    } break;
                    case 2: {
                    } break;
                    case 3: {
                    } break;
                    case 4: {
                    } break;
                    case 5: {
                    } break;
                    case 6: {
                    } break;
                    case 7: {
                    } break;

                }

            } else {
                // bit is not set
                switch (i) {
                    case 0: {
                        b3.css({
                            "background-color": "transparent"
                        });
                    } break;
                    case 1: {
                        b4.css({
                            "background-color": "transparent"
                        });
                    } break;
                    case 2: {
                    } break;
                    case 3: {
                    } break;
                    case 4: {
                    } break;
                    case 5: {
                    } break;
                    case 6: { 
                    } break;
                    case 7: {
                    } break;

                }
            }

        }

    };




    /*    var Model = function () {
            var self = this;
            self.message = ko.observable("");
            self.messages = ko.observableArray();
        };
    
    */
}); 