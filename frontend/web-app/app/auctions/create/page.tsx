import Heading from "@/app/components/Heading";
import AuctionForm from "@/app/auctions/AuctionForm";

export default function AuctionCard() {
  return (
      <div className="mx-auto shadow-lg p-10 bg-white rounded-lg">
        <Heading title="Sell your car!" subtitle={"Fill in the details of your car and start the auction."}/>
        <AuctionForm/>
      </div>
  );
}