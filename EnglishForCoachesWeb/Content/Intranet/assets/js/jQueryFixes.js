$.validator.setDefaults({
    highlight: function (element) {
        $(element).parents('div:first').addClass("has-error");
    },
    unhighlight: function (element) {
        $(element).parents('div:first').removeClass("has-error");
    }
});

$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
};

$.validator.methods.number = function(value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
};


// Use $.getJSON instead of $.get if your server is not configured to return the
// right MIME type for .json files.
$.when(
  $.getJSON("/Scripts/cldr/main/es/currencies.json"),
  $.getJSON("/Scripts/cldr/main/es/numbers.json"),
  $.getJSON("/Scripts/cldr/main/es/timeZoneNames.json"),
  $.getJSON("/Scripts/cldr/main/es/dateFields.json"),
  $.getJSON("/Scripts/cldr/main/es/ca-generic.json"),
  $.getJSON("/Scripts/cldr/main/es/ca-gregorian.json"),
  $.getJSON("/Scripts/cldr/supplemental/likelySubtags.json"),
  $.getJSON("/Scripts/cldr/supplemental/timeData.json"),
  $.getJSON("/Scripts/cldr/supplemental/weekData.json")
).then(function () {
    // Normalize $.get results, we only need the JSON, not the request statuses.
    return [].slice.apply(arguments, [0]).map(function (result) {
        return result[0];
    });

}).then(Globalize.load).then(function () {
    $.validator.methods.date = function (value, element) {
        console.log("Globalize");
        var es=new Globalize("es-ES");
        // you can alternatively pass the culture to parseDate instead of
        // setting the culture above, like so:
        // parseDate(value, null, "en-AU")
        return this.optional(element) || es.parseDate(value) !== null;
    };

});
