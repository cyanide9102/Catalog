"use strict";

$(document).ready(function () {
    $("#booksTable").DataTable({
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
            url: "/Book/GetBookList",
            type: "POST",
        },
        columns: [
            {
                data: "title",
                name: "Title"
            },
            {
                data: "price",
                name: "Price",
                render: function (data, _type, _row, _meta) {
                    return `$${data}`;
                }
            },
            {
                data: "authorLinks",
                name: "AuthorLinks",
                orderable: false,
                render: function (data, _type, _row, _meta) {
                    var authors = "";
                    for (var link of data) {
                        authors += link.author.name;
                        if (data.indexOf(link) + 1 != data.length) {
                            authors += ", ";
                        }
                    }
                    return authors;
                }
            },
            {
                data: "genreLinks",
                name: "GenreLinks",
                orderable: false,
                render: function (data, _type, _row, _meta) {
                    var genres = "";
                    for (var link of data) {
                        genres += link.genre.name;
                        if (data.indexOf(link) + 1 != data.length) {
                            genres += ", ";
                        }
                    }
                    return genres;
                }
            },
            {
                data: "publisher",
                name: "Publisher",
                render: function (data, _type, _row, _meta) {
                    if (!data) {
                        return "Unknown";
                    }
                    return data.name;
                }
            },
            {
                data: "publishedOn",
                name: "PublishedOn",
                render: function (data, _type, _row, _meta) {
                    if (!data) {
                        return "";
                    }
                    return moment(new Date(data)).format('Do MMMM, YYYY');
                }
            },
            {
                data: "id",
                name: "Actions",
                orderable: false,
                searchable: false,
                className: "text-end",
                render: function (data, _type, _row, _meta) {
                    return `<a href="/Book/Info/${data}" class="btn btn-sm btn-info">View</a>
                            <a href="/Book/Edit/${data}" class="btn btn-sm btn-secondary">Edit</a>
                            <form asp-area="" asp-controller="Book" asp-action="Delete" method="post" class="d-inline">
                                <input type="hidden" name="Id" value="${data}" />
                                <button type="submit" class="btn btn-sm btn-danger">Delete</button>
                            </form>`;
                }
            },
        ]
    });
});
