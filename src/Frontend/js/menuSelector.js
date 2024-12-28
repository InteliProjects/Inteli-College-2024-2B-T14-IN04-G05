var menuItem = document.querySelectorAll('.item-menu');

function selectLink(){
    menuItem.forEach(item => item.classList.remove('ativo'));
    this.classList.add('ativo');
    
}

menuItem.forEach(item => item.addEventListener('click', selectLink));

var menuIcon = document.querySelectorAll('.item-menu');

function selectLink() {
    // Remove a classe 'ativo' de todos os itens
    menuIcon.forEach(item => item.classList.remove('ativo'));

    // Adiciona a classe 'ativo' ao item clicado
    this.classList.add('ativo');
}

// Adiciona o evento de clique a cada item do menu
menuIcon.forEach(item => item.addEventListener('click', selectLink));
