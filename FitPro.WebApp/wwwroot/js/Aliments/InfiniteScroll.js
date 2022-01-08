$(document).ready(function () {
    var InfinityScrollUtils = {
        page: 2,
        ticking: false,
        getScrollPosition: function () {
            var s = $(window).scrollTop(),
                d = $(document).height(),
                c = $(window).height();
            return (s / (d - c)) * 100;
        },

        addAliment: function (item) {
            var role = $('#currentUserRole').val();

            var tableBody = $("#alimentsList");
            var newRow = document.createElement("tr");
            newRow.innerHTML = '<td>' + item.name + '</td>'
                + '<td>' + item.calories + '</td>'
                + '<td>' + item.protein + '</td>'
                + '<td>' + item.carbo + '</td>'
                + '<td>' + item.fat + '</td>'
            if (role == "Nutritionist") {
                newRow.innerHTML = newRow.innerHTML +
                    '<td><a asp-action="EditAliment" asp-controller="Nutritionist" asp-route-alimentName="' + item.name + '"><i class="fas fa-edit"></i></a>'
                    + '<a asp-action="DeleteAliment" asp-controller="Nutritionist" asp-route-alimentName="' + item.name + '"><i class="fas fa-trash"></i></a></td>';
            }
           tableBody.append(newRow);
        }
    }

    $(showMore).click(function () {
         if (!InfinityScrollUtils.ticking) {
                $.ajax({
                    url: "/Nutritionist/GetAlimentsList",
                    data: {
                        currentPage: InfinityScrollUtils.page,
                        searchString: $('#search-input-A').val()
                    }
                }).done(function (data) {
                    data.forEach(function (item) {
                        InfinityScrollUtils.addAliment(item);
                    })
                    InfinityScrollUtils.page = InfinityScrollUtils.page + 1;
                    if (data.length != 15) {
                        $(showMore).remove();
                    }
                })
            }
    })
})