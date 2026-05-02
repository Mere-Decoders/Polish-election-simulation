export async function loadColors() {
  const response = await fetch('/colors.json');
  return response.json();
}