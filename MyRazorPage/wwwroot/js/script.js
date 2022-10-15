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

    for (let i = 1; i < totalPage; i++) {
        params.set("pageNum", i);
        if (i == pageNum) {
            divPagingElement.innerHTML += `<a href="${url}" class="active">${i}</a>`;
            continue;
        }
        divPagingElement.innerHTML += `<a href="${url}">${i}</a>`;
    }
   
}