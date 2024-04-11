import Listings from "./auctions/Listings";
import PostList from "@/app/post/PostList";

export default function Home() {
  return (
      <div>
        <div>
          <PostList/>
        </div>
        <Listings/>
      </div>
  )
}
