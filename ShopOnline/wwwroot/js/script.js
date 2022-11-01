function paging(totalPage) {
    let gap = 2;
    let divPagingElement = document.querySelector("#pagination");

    let url = new URL(location.href);
    let params = url.searchParams;

    let pageNum;
    if (params.has("pageNum")) {
        pageNum = Number(params.get("pageNum"));
    } else {
        pageNum = 1;
    }

    if (pageNum > 1) {
        params.set("pageNum", 1);
        divPagingElement.innerHTML += `<a href="${url}">First</a>`;
        params.set("pageNum", pageNum - 1);
        divPagingElement.innerHTML += `<a href="${url}">&laquo;</a>`;
    }
    let pageFront = pageNum - gap;
    if (pageFront < 1) {
        pageFront = 1;
    }
    for (let i = pageFront; i < pageNum; i++) {
        params.set("pageNum", i);

        divPagingElement.innerHTML += `<a href="${url}">${i}</a>`;
    }

    // print current page;
    divPagingElement.innerHTML += `<a href="javascript:void" class="active">${pageNum}</a>`;

    let pageRear = pageNum + gap;
    if (pageRear > totalPage) {
        pageRear = totalPage;
    }
    for (let i = pageNum + 1; i <= pageRear; i++) {
        params.set("pageNum", i);

        divPagingElement.innerHTML += `<a href="${url}">${i}</a>`;
    }

    if (pageNum < totalPage) {
        params.set("pageNum", Number(pageNum) + 1);
        divPagingElement.innerHTML += `<a href="${url}">&raquo;</a>`;

        params.set("pageNum", Number(totalPage));
        divPagingElement.innerHTML += `<a href="${url}">Last</a>`;
    }
}

function filterNsearch(event, elementId, paramKey) {
    event.preventDefault();
    const element = document.getElementById(elementId);
    const url = new URL(location.href);
    url.searchParams.delete("pageNum");
    url.searchParams.set(paramKey, element.value);
    location.href = url;
}

function SelectSortBy(ele) {
    var url = new URL(window.location.href);
    if (ele.value.length === 0) {
        url.searchParams.delete("sortBy");
    }
    if (url.searchParams.has("sortBy")) {
        url.searchParams.set("sortBy", ele.value);
    } else {
        url.searchParams.append("sortBy", ele.value);
    }
    window.location.href = url;
}

function DeactiveCustomer(ele, customerid) {
    var active = true;
    if (ele.innerHTML === 'Deactive') {
        active = false;
    } else if (ele.innerHTML === 'Active') {
        active = true;
    }

    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", '/admin/customers/details');


    const idField = document.createElement('input');
    idField.type = 'hidden';
    idField.name = 'id';
    idField.value = customerid;
    form.appendChild(idField);
    const activeField = document.createElement('input');
    activeField.type = 'hidden';
    activeField.name = 'active';
    activeField.value = active;
    form.appendChild(activeField);
    document.body.appendChild(form);
    form.submit();
}