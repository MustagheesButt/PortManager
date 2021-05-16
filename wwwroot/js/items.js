const itemsSelector = document.querySelector('#items-selector')
const selectedItems = document.querySelector('#selected-items')

itemsSelector.addEventListener('change', function () {
    const item = document.createElement('tr')
    const nameCell = document.createElement('td')
    nameCell.setAttribute('class', 'item-name')
    nameCell.innerHTML = itemsSelector.options[this.selectedIndex].text

    const itemCell = document.createElement('td')
    const i = document.createElement('input')
    i.setAttribute('type', 'hidden')
    i.setAttribute('name', 'items[]')
    i.setAttribute('value', itemsSelector.value)
    itemCell.appendChild(i)

    const quantityCell = document.createElement('td')
    const q = document.createElement('input')
    q.setAttribute('type', 'number')
    q.setAttribute('min', '0')
    q.setAttribute('name', 'quantities[]')
    q.setAttribute('value', 0)
    quantityCell.appendChild(q)

    const deleteBtn = document.createElement('td')
    deleteBtn.setAttribute('class', 'delete-btn')
    deleteBtn.innerHTML = "&nbsp;&nbsp;&nbsp;&cross;"

    item.appendChild(nameCell)
    item.appendChild(itemCell)
    item.appendChild(quantityCell)
    item.appendChild(deleteBtn)
    selectedItems.appendChild(item)

    itemsSelector.options[this.selectedIndex].remove()
})

document.addEventListener('click', function (e) {
    const target = e.target
    if (target.classList.contains('delete-btn')) {
        const item = target

        const temp = document.createElement('option')
        temp.setAttribute('value', item.parentElement.querySelector('input[name^=items]').value)
        temp.innerHTML = item.parentElement.querySelector('.item-name').innerHTML
        itemsSelector.appendChild(temp)

        item.parentElement.remove()
    }
})