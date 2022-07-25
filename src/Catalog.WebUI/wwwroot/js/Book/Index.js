'use strict';

$(document).ready(function () {
    $('#booksTable').DataTable({
        ajax: {
            url: "/Book/GetBookList",
            type: "POST",
        },
        processing: true,
        serverSide: true,
        filter: true,
        columns: [
            { data: "title", name: "Title" },
            { data: "price", name: "Price" },
            { data: "price", name: "Price" },
            { data: "price", name: "Price" },
            { data: "publishedOn", name: "PublishedOn" },
            { data: "createdAt", name: "createdAt" },
            { data: "updatedAt", name: "UpdatedAt" },
            { data: "id", name: "Id" },
        ]
    });
});
