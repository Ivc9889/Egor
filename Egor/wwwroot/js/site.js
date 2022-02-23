let deptItems = document.querySelectorAll(".dept-item__title");
deptItems.forEach(deptItem => {
    deptItem.onclick = function () {
        this.classList.toggle("show");
    }
});

let tableItems = document.querySelectorAll(".table-head");
tableItems.forEach(tableItem => {
    tableItem.onclick = function () {
        this.classList.toggle("show");
    }
});

let check = document.querySelector(".choose-file input");
check.onclick = function () {
    if (this.value == "false") {
        this.value = true;
        document.querySelector(".new-file").style.display = "block";
        document.querySelector(".new-file input").required = true;
    } else {
        this.value = false;
        document.querySelector(".new-file").style.display = "none";
        document.querySelector(".new-file input").required = false;
    }
}