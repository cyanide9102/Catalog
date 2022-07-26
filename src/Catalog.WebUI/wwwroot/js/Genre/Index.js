"use strict";

$(document).ready(function () {
    $("#genresTable").DataTable({
        processing: true,
        serverSide: true,
        filter: true,
        paging: true,
        searching: {
            regex: false
        },
        ajax: {
            url: "/Genre/GetGenreList",
            type: "POST",
        },
        columns: [
            {
                data: "name",
                name: "Name"
            },
            {
                data: "bookLinks",
                name: "BookLinks",
                orderable: false,
                searchable: false,
                render: function (data, _type, _row, _meta) {
                    return data.length;
                }
            },
            {
                data: "createdAt",
                name: "CreatedAt",
                searchable: false,
                render: function (data, _type, _row, _meta) {
                    if (!data) {
                        return "";
                    }
                    return moment(new Date(data)).format('Do MMMM, YYYY h:mm a');
                }
            },
            {
                data: "updatedAt",
                name: "UpdatedAt",
                searchable: false,
                render: function (data, _type, _row, _meta) {
                    if (!data) {
                        return "";
                    }
                    return moment(new Date(data)).format('Do MMMM, YYYY h:mm a');
                }
            },
            {
                data: "id",
                name: "Actions",
                orderable: false,
                searchable: false,
                className: "text-end",
                render: function (data, _type, _row, _meta) {
                    return `<a href="/Genre/Info/${data}" class="btn btn-sm btn-info">View</a>
                            <a href="/Genre/Edit/${data}" class="btn btn-sm btn-secondary">Edit</a>
                            <a href="/Genre/Info/${data}" class="btn btn-sm btn-danger">Delete</a>`;
                }
            },
        ]
    });
});
