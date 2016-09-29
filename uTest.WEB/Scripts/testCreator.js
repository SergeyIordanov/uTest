function addAnswer(question, answer) {
    if (answer > 2)
        $("#remove-answer_" + question + "_" + (answer - 1)).remove();
    $("#addAnswerWrapper_" + question).remove();
    var selector = "#answer_" + question + "_" + (answer - 1);
    var html = "<div id='answer_" + question + "_" + answer + "' class='row'>";
    html += "<div class='input-field col s6'>";
    html += "<label>Answer #" + answer + "</label>";
    html += "<input type='text' name='answer_" + question + "_" + answer + "' required/></div>";
    html += " <div id='wrapper-checkbox_" + question + "_" + answer + "' class='input-field col s3'><p>";
    html += "<input type='checkbox' id='correct_" + question + "_" + answer + "' name='correct' value='" + question + "_" + answer + "'/>";
    html += "<label for='correct_" + question + "_" + answer + "'><span class='hide-on-small-and-down'>is correct</span></label></p></div>";
    html += "<div  id='remove-answer_" + question + "_" + answer + "' class='input-field col s3'>";
    html += "<p class='active-par remove-answer red-text' onclick='removeAnswer(" + question + ", " + answer + ")'>" +
        "<i class='material-icons'>clear</i></p></div></div>";
    if (answer < 12) {

        html += "<div id='addAnswerWrapper_" + question + "' class='input-field col s12 no-margin-top'>";
        html += "<p class='flow-text sub-welcome-text blue-text active-par' onclick='addAnswer(" + question + ", " + (answer + 1) + ")'>+ add answer</p></div>";        
    }
    $(selector).after(html);
}

function addQuestion(question) {
    if (question > 2)
        $("#remove-question_" + question).remove();
    $("#addQuestionWrapper").remove();
    var selector = "#question_" + (question - 1);
    var html = "<div id='question_" + question + "' class='row no-margin-bot'>";
    html += "<div id='inner-wrapper-question_" + question + "' class='col s12'>";
    html += "<p class='flow-text avarage-text centralized'>Question #" + question +
        "<span id='remove-question_" + question + "'  class='active-par remove-question red-text'  onclick='removeQuestion(" + question + ")'>" +
        "<i class='material-icons'>clear</i></span></p>";
    html += "<div class='input-field col s12'><label>Question</label>";
    html += "<input type='text' name='question_" + question + "' required/></div>";
    html += "<div id='answer_" + question + "_1' class='row'>";
    html += "<div class='input-field col s6'><label>Answer #1</label>";
    html += "<input type='text' name='answer_" + question + "_1' required/></div>";
    html += "<div class='input-field col s3'><p>";
    html += "<input type='checkbox' id='correct_" + question + "_1' name='correct' value='" + question + "_1'/>";
    html += "<label for='correct_" + question + "_1'><span class='hide-on-small-and-down'>is correct</span></label></p></div></div>";
    html += "<div id='addAnswerWrapper_" + question + "' class='input-field col s12 no-margin-top'>";
    html += "<p class='flow-text sub-welcome-text blue-text active-par' onclick='addAnswer(" + question + ", 2)'>+ add answer</p></div></div></div>";
    if (question < 50) {
        html += "<div id='addQuestionWrapper' class='col s12'>";
        html += "<p class='flow-text avarage-text blue-text active-par' onclick='addQuestion(" + (question + 1) + ")'>+ add question</p></div>";
    }
    $(selector).after(html);
}

function removeQuestion(question) {
    $("#addQuestionWrapper").remove();
    var wrapper = "<div id='addQuestionWrapper' class='col s12'>";
    wrapper += "<p class='flow-text avarage-text blue-text active-par' onclick='addQuestion(" + question + ")'>+ add question</p></div>";
    $("#question_" + (question - 1)).after(wrapper);

    if (question > 2) {
        var delLabel = "<span id='remove-question_" + question + "'  class='active-par remove-question red-text'  onclick='removeQuestion(" + question + ")'>" +
        "<i class='material-icons'>clear</i></span>";
        $("#inner-wrapper-question_" + question).after(delLabel);
    }
    $("#question_" + question).remove();
}

function removeAnswer(question, answer) {
    $("#addAnswerWrapper_" + question).remove();
    var wrapper = "<div id='addAnswerWrapper_" + question + "' class='input-field col s12 no-margin-top'>";
    wrapper += "<p class='flow-text sub-welcome-text blue-text active-par' onclick='addAnswer(" + question + ", " + answer + ")'>+ add answer</p></div>";
    $("#answer_" + question + "_" + (answer - 1)).after(wrapper);

    if (answer > 2){
        var delLabel = "<div  id='remove-answer_" + question + "_" + (answer - 1) + "' class='input-field col s3'>";
        delLabel += "<p class='active-par remove-question red-text' onclick='removeAnswer(" + question + ", " + (answer - 1) + ")'>" +
            "<i class='material-icons'>clear</i></p></div>";
        $("#wrapper-checkbox_" + question + "_" + (answer - 1)).after(delLabel);
    }
    $("#answer_" + question + "_" + answer).remove();
}

function onShowAllChanged() {
    if ($("#showAll").prop("checked"))
        $("#questionsToShow").attr("disabled", "disabled");
    else
        $("#questionsToShow").removeAttr("disabled");
}