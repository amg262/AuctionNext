import React from "react";
import {Post, PostComment} from "@/types";
import {getPostComments} from "@/app/actions/auctionActions";

type Props = {
  userId: string,
  post: Post
}

export default async function PostComments({userId, post}: Props) {
  let error = null;
  let postComments: PostComment[] = [];

  try {
    postComments = await getPostComments(post?.id);
    // post = postData;
    console.log('comment - comp', postComments)
  } catch (err: any) {
    error = err;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
      <div>
        <div className='mt-6'>
          <h2 className='text-2xl font-bold text-gray-800'>Comments:</h2>
          {postComments.length > 0 ? (
              <ul>
                {postComments.map((comment, index) => (
                    <li key={index} className="bg-gray-100 rounded p-3 my-2">
                      <p>{comment.content}</p>
                      <p className="text-gray-500 text-sm">Posted by: {comment.userId}</p>
                    </li>
                ))}
              </ul>
          ) : (
              <p>No comments yet.</p>
          )}
        </div>
      </div>
  );
}