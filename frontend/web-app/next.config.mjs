/** @type {import('next').NextConfig} */
const nextConfig = {
  images: {
    // domains: ['cdn.pixabay.com'], // This is deprecated
    remotePatterns: [
      {
        protocol: 'https',
        hostname: '**',
      },
    ],// This is the new way
  },
};

export default nextConfig;
