export function numberWithCommas(amount: number) {
  console.log(amount);
  return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}