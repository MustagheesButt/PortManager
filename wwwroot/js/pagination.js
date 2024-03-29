﻿let currPage = 1
const pageSize = 10
const paginationItems = document.querySelectorAll('.pagination-item')

const filterBy = document.querySelector('#filterBy')

function filteredPaginationItems() {
    return [...paginationItems]
        .filter(paginationItem =>
            [...paginationItem.querySelectorAll('[data-filter]')]
                .some(item => item.innerHTML.toLowerCase().indexOf(filterBy.value.toLowerCase()) !== -1)
        )
}

function totalPages() {
    return filteredPaginationItems().length / pageSize
}

function renderPaginatedData() {
    const data = filteredPaginationItems()
    paginationItems.forEach((ele) => {
        ele.className += ' d-none'
    })
    const start = (currPage - 1) * pageSize
    const end = Math.min(data.length, (currPage - 1) * pageSize + (pageSize - 1))
    for (let i = start; i < end; i++) {
        data[i].classList.remove('d-none')
    }
}
function renderPagination() {
    const paginationContainers = document.querySelectorAll('.pagination')
    const N = totalPages()
    paginationContainers.forEach((ele) => {
        ele.innerHTML = ''
        if (currPage > 1) {
            ele.appendChild(paginationButton('Previous', currPage - 1))
        }

        for (let i = 0; i < N; i++) {
            ele.appendChild(paginationButton(i + 1, i + 1))
        }

        if (currPage < N) {
            ele.appendChild(paginationButton('Next', currPage + 1))
        }
    })
}

function paginationButton(text, index) {
    const li = document.createElement('li')
    const a = document.createElement('a')
    a.className = 'page-link'
    a.href = '#'
    a.setAttribute('data-index', index)
    a.innerHTML = text
    a.addEventListener('click', (e) => {
        currPage = +e.target.getAttribute('data-index')
        renderPagination()
        renderPaginatedData()
    })

    li.className = 'page-item'
    li.appendChild(a)
    return li
}

renderPagination()
renderPaginatedData()

filterBy.addEventListener('input', function () {
    renderPagination()
    renderPaginatedData()
})