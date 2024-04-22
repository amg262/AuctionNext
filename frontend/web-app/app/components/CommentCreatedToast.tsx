import {Post, PostComment} from '@/types'
import Image from 'next/image'
import Link from 'next/link'
import React from 'react'

type Props = {
  post: Post
  postComment: PostComment
}

export default function CommentCreatedToast({post, postComment}: Props) {
  console.log('postComment - toast', postComment)
  return (
      <Link href={`/post/details/${post.id}`} className='flex flex-col items-center'>
        <div className='flex flex-row items-center gap-2'>
          <Image
              src="/comment.png"
              alt='image'
              height={80}
              width={80}
              className='rounded-lg w-auto h-auto'
          />
          <span>New Comment on {post.title}</span>
        </div>
      </Link>
  )
}
