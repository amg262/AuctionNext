import Listings from "./auctions/Listings";
import PostList from "@/app/post/PostList";
import Footer from "@/app/components/Footer";

export default function Home() {
  return (
      <div>
        <div>
          <PostList/>
        </div>
        <br/><br/>
        <Listings/>
        <Footer/>
      </div>
  )
}
