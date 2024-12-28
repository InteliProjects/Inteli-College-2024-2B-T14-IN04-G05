// Exporta a função `App` como padrão para que possa ser importada em outros arquivos.
export default function App() {
  return (
    // Retorna um elemento <h1> com classes do Tailwind CSS aplicadas.
    // "text-3xl" define o tamanho do texto para 3xl.
    // "font-bold" aplica negrito ao texto.
    // "underline" sublinha o texto.
    <h1 className="text-3xl font-bold underline">
      NavBar! {/* Texto que será exibido como título da aplicação */}
    </h1>
  )
}
