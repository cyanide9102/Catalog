"use strict";

$(document).ready(function () {

    jQuery.fn.dataTable.Api.register('processing()', function (show) {
        return this.iterator('table', function (ctx) {
            ctx.oApi._fnProcessingDisplay(ctx, show);
        });
    });

    var table = $("#booksTable").DataTable({
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
            error: function (e) {
                console.error({ ...e.responseJSON });
                table.processing(false);
            }
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
                searchable: false,
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
                searchable: false,
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
                name: "Publisher.Name",
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
        ],
        initComplete: function () {
            $('#booksTableFilters').on('keyup change clear', '#searchByTitle', function () {
                if (table.columns(0).search() != this.value) {
                    table.columns(0).search(this.value).draw();
                }
            });

            $('#booksTableFilters').on('keyup change clear', '#searchByPrice', function () {
                if (table.columns(1).search() != this.value) {
                    table.columns(1).search(this.value).draw();
                }
            });

            $('#booksTableFilters').on('keyup change clear', '#searchByPublisher', function () {
                if (table.columns(4).search() != this.value) {
                    table.columns(4).search(this.value).draw();
                }
            });

            /*
            $('#booksTableFilters').on('submit', function (e) {
                e.preventDefault();

                var searchByTitle = $('#searchByTitle').val();
                if (!!searchByTitle && table.columns(0).search() != searchByTitle) {
                    table.columns(0).search(searchByTitle).draw();
                } else {
                    table.columns(0).search('').draw();
                }

                var searchByPrice = $('#searchByPrice').val();
                if (!!searchByPrice && table.columns(0).search() != searchByPrice) {
                    table.columns(1).search(searchByPrice).draw();
                } else {
                    table.columns(1).search('').draw();
                }

                var searchByPublisher = $('#searchByPublisher').val();
                if (!!searchByPublisher && table.columns(0).search() != searchByPublisher) {
                    table.columns(4).search(searchByPublisher).draw();
                } else {
                    table.columns(4).search('').draw();
                }
            });
            */

            /*
            // Setup - add a text input to each footer cell
            $("#booksTable tfoot th.searchable-text").each(function () {
                var title = $(this).text();
                $(this).html(`<input type="text" placeholder="${title}" />`);
            });
            $("#booksTable tfoot th.searchable-number").each(function () {
                var title = $(this).text();
                $(this).html(`<input type="number" placeholder="${title}" />`);
            });
            $("#booksTable tfoot th.searchable-date").each(function () {
                var title = $(this).text();
                $(this).html(`<input type="date" />`);
            });
            // Apply the search
            this.api()
                .columns()
                .every(function () {
                    var self = this;
                    console.log({ ...this });
                    console.log(this.footer());
                    $('input', this.footer()).on('keyup change clear', function () {
                        console.log(self.search());
                        console.log(this.value);
                        if (self.search() !== this.value) {
                            self.search(this.value).draw();
                        }
                    });
                });
            */
        },
    });
});
