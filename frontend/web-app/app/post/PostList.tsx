import {getPosts} from "@/app/actions/auctionActions";

export default async function PostList() {
  const posts = await getPosts();

  return (
      <div>


      </div>
  );
}