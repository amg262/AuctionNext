import {getPosts} from "@/app/actions/auctionActions";
import PostCard from "@/app/post/PostCard";
import {Post} from "@/types";

export default async function PostList() {
  const posts = await getPosts();

  return (
      <div>
        <h2 className="text-2xl font-bold mb-6">Post List</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {posts.map((post: Post) => (
              <PostCard post={post} key={post.guid}/>
          ))}
        </div>
      </div>
  );
}