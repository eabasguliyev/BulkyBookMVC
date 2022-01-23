﻿let dataTable = null;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $("#tblData").DataTable(
        {
            "ajax": {
                "url": "/Admin/Company/GetAll",
            },
            "columns": [
                { "data": "name", "width": "15%"},
                { "data": "streetAddress", "width": "15%"},
                { "data": "city", "width": "15%"},
                { "data": "state", "width": "15%"},
                { "data": "phoneNumber", "width": "15%"},
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="w-100 btn-group text-center" role="group">
                                <a href="/Admin/Company/Upsert?id=${data}" class="btn btn-primary" style="width:150px;">
                                    <i class="bi bi-pencil-square mx-2"></i> Edit
                                </a>
                                <a  onclick="deleteProduct('/Admin/Company/Delete/${data}')" class="btn btn-danger" style="width:150px;">
                                    <i class="bi bi-trash-fill"></i> Delete
                                </a>
                            </div>
                        `;
                    },
                    "width": "15%"
                },
            ]
        }
    );
}

function deleteProduct (url){
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}