var serviceUrl = 'https://localhost:44350/api/UnderTheDome';

$(function () {
    //Get all actors on page load
    $.ajax({
        url: serviceUrl,
        method: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            LoadActors(data);
            addClickListeners();
        }
    });

    function LoadActors(data) {
        
        $.each(data, function (i, item) {
            var thisNote = item.note;
            var noteToAssign;
            if (thisNote == null) {
                noteToAssign = "";
            }
            else {
                noteToAssign = thisNote.actorNote;
            }
            var actorButton =
                '<div class="actor-button-div">' +
                    '<button type="button" class="btn btn-secondary btn-lg btn-block">' + item.id + '</button>' +
                    '<div class="actor-invisible actor-toggle">' +
                        '<h3>Name: ' + item.actorName + '</h3>' +
                        '<h3>Birthday: ' + item.birthday + '</h3>' +
                        '<h3>Gender: ' + item.gender + '</h3>' +
                        '<h3 class="actor-note">Note: ' + noteToAssign + '</h3>' +
                        '<div class="row">' +
                            '<div class="col-md-2">' +
                                '<button type="button" class="btn btn-info btn-lg new-note-btn">New note</button></div>' +
                            '<div class="col-md-2 button-div">' +
                                '<button type="button" class="btn btn-danger btn-lg delete-actor-btn">Delete actor</button></div></div>' +
                            '<div class="add-note-invisible note-toggle">' +
                                '<input type="text" name="Note" id="note" class="form-control note-input" placeholder="Note">' +
                                '<button type="button" class="btn btn-outline-info btn-lg add-note-btn">Add note</button></div></div></div>'
            $('#result').append(actorButton);
        });
    };

    function addClickListeners() {
        //Hide/Un-hide actors on click of it's button
        $('.btn-block').on('click', function () {
            var div = $(this).parent().find(".actor-toggle");
            div.toggleClass("actor-invisible");
            console.log("clicked");
        });
        //Un-hide text input and for new note and button to add it
        $('.new-note-btn').on('click', function () {
            var div1 = $(this).closest(".actor-toggle");
            div1.find(".note-toggle").removeClass("add-note-invisible");
        });
        //Post the new note to API
        $('.add-note-btn').on('click', function () {
            var actorIdForNewNote = $(this).closest(".actor-button-div").children("button").text();
            var newActorNote = $(this).closest(".note-toggle").children("input").val();
            var Note = { actorId: actorIdForNewNote, actorNote: newActorNote };
            $(this).closest(".actor-toggle").children(".actor-note").text("Note: " + newActorNote);
            $.ajax({
                url: serviceUrl,
                method: 'POST',
                data: Note,
                success: function (data) {
                }
            });
        });

        $('.delete-actor-btn').on('click', function () {
            var divToDelete = $(this).closest(".actor-button-div");
            var actorIdToDelete = divToDelete.children("button").text();
            $.ajax({
                url: serviceUrl + '/actor/' + actorIdToDelete,
                method: 'DELETE',
                data: actorIdToDelete,
                success: function () {
                    divToDelete.css("display", "none");
                }
            })
        })
    }
});

/*$(window).on("load", function () {

    
    
    
});*/

