import {Post} from '@/types'
import Image from 'next/image'
import Link from 'next/link'
import React from 'react'

type Props = {
  post: Post
}

export default function PostCreatedToast({post}: Props) {
  return (
      <Link href={`/post/details/${post.id}`} className='flex flex-col items-center'>
        <div className='flex flex-row items-center gap-2'>
          <Image
              src="/post.png"
              alt='image'
              height={80}
              width={80}
              className='rounded-lg w-auto h-auto'
          />
          <span>New Post by {post.userId}</span>
        </div>
      </Link>
  )
}
