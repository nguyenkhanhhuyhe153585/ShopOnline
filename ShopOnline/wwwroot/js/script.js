function paging(totalPage) {
    let divPagingElement = document.querySelector("#pagination");

    let url = new URL(location.href);
    let params = url.searchParams;

    let pageNum;
    if (params.has("pageNum")) {
        pageNum = params.get("pageNum");
    } else {
        pageNum = 1;
    }

    if (pageNum > 1) {
        params.set("pageNum", pageNum - 1);
        divPagingElement.innerHTML += `<a href="${url}">&laquo;</a>`;
    }
      
    for (let i = 1; i <= totalPage; i++) {
        params.set("pageNum", i);
        if (i == pageNum) {
            divPagingElement.innerHTML += `<a href="javascript:void" class="active">${i}</a>`;
            continue;
        }
        divPagingElement.innerHTML += `<a href="${url}">${i}</a>`;
    }

    if (pageNum < totalPage) {
        params.set("pageNum", Number(pageNum) + 1);
        divPagingElement.innerHTML += `<a href="${url}">&raquo;</a>`;
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