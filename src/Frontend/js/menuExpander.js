// Seleciona o botão de expansão pelo seu ID `btn-exp` e armazena na variável `btnExp`.
var btnExp = document.querySelector('#btn-exp');

// Seleciona o elemento do menu lateral pela sua classe `menu-lateral` e armazena na variável `sideMenu`.
var sideMenu = document.querySelector('.menu-lateral');

// Adiciona um evento de clique ao botão `btnExp`.
btnExp.addEventListener('click', function() {
    // Quando o botão for clicado, alterna a classe `expandir` no elemento `sideMenu`.
    // Se a classe `expandir` estiver presente, ela será removida; se não estiver, será adicionada.
    sideMenu.classList.toggle('expandir');
});
