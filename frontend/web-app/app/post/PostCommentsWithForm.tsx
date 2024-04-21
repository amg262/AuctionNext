'use client';

import React, {useEffect} from "react";
import {Post, PostComment} from "@/types";
import {createComment, getPostComments} from "@/app/actions/auctionActions";

type Props = {
  username?: string,
  post: Post
}

export default function PostCommentsWithForm({username, post}: Props) {
  const [postComments, setPostComments] = React.useState<PostComment[]>([]);
  const [error, setError] = React.useState(null);

  const [comment, setComment] = React.useState('');

  const handleChange = (event: { target: { value: React.SetStateAction<string>; }; }) => {
    setComment(event.target.value);
  };

  const handleSubmit = async (event: { preventDefault: () => void; }) => {
    event.preventDefault();

    console.log('postId', post.id)
    console.log('comment', comment)

    try {
      // Assuming you have a function to call your API
      const response = await createComment(post.id, {content: comment, userId: username});
      // You might want to update the local state to show the comment immediately
      console.log('Comment created', response);
      setPostComments([...postComments, {postId: post.id, content: comment, userId: username}]);
      setComment('');
    } catch (error) {
      console.error('Failed to post comment', error);
    }
  }

  useEffect(() => {

    async function fetchPostComments() {
        try {
          const comments = await getPostComments(post.id);
          setPostComments(comments);
        } catch (error) {
          console.error('Failed to fetch post comments', error);
        }
    }

    fetchPostComments();

  }, [post.id, comment, postComments.length]);


  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
      <div>

        <div className='mt-4'>
          <form onSubmit={handleSubmit}>
        <textarea
            className="w-full p-2 border rounded"
            placeholder="Add a comment..."
            value={comment}
            onChange={handleChange}
            rows={4}
        ></textarea>
            <button type="submit" className="mt-2 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
              Post Comment
            </button>
          </form>
        </div>
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