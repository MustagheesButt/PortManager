let currPage = 1
const pageSize = 10
const users = document.querySelectorAll('.pagination-item')
const totalPages = users.length / pageSize

function renderPaginatedData() {
    users.forEach((ele) => {
        ele.className += ' d-none'
    })
    const start = (currPage - 1) * pageSize
    const end = Math.min(users.length, (currPage - 1) * pageSize + (pageSize - 1))
    for (let i = start; i < end; i++) {
        users[i].classList.remove('d-none')
    }
}
function renderPagination() {
    const paginationContainers = document.querySelectorAll('.pagination')
    paginationContainers.forEach((ele) => {
        ele.innerHTML = ''
        if (currPage > 1) {
            ele.appendChild(paginationButton('Previous', currPage - 1))
        }

        for (let i = 0; i < totalPages; i++) {
            ele.appendChild(paginationButton(i + 1, i + 1))
        }

        if (currPage < totalPages) {
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