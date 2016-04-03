// Copyright 2016 the pokefans authors. See copying.md for legal info.
$('#pokemonselect').select2({
    ajax: {
        url: '/api/pokemon/names',
        delay: 250,
        data: function (params) {
            return {
                query: params.term, // search term
                page: params.page
            };
        },
        processResults: function (data) {
            return {
                results: $.map(data, function (Name, Id) {
                    return {
                        text: Name,
                        id: Id
                    }
                })
            };
        }
    }
});