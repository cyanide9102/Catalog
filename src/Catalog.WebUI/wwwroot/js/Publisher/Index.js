"use strict";

$(document).ready(function () {
    $("#publishersTable").DataTable({
        paging: true,
        processing: true,
        serverSide: true,
        search: {
            return: true
        },
        searching: {
            regex: false
        },
        ajax: {
            url: "/Publisher/GetPublisherList",
            type: "POST",
        },
        columns: [
            {
                data: "name",
                name: "Name"
            },
            {
                data: "country",
                name: "Country"
            },
            {
                data: "books",
                name: "Books",
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
                    return `<a href="/Publisher/Info/${data}" class="btn btn-sm btn-info">View</a>
                            <a href="/Publisher/Edit/${data}" class="btn btn-sm btn-secondary">Edit</a>`;
                            //<a href="/Publisher/Info/${data}" class="btn btn-sm btn-danger">Delete</a>`;
                }
            },
        ]
    });
});
