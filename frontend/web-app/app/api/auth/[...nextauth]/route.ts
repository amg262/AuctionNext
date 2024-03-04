import NextAuth, {NextAuthOptions} from "next-auth";
import DuendeIdentityServer6 from 'next-auth/providers/duende-identity-server6';

/**
 * Configuration options for NextAuth, including session management, providers, and callbacks.
 * This configuration uses Duende Identity Server 6 as an authentication provider.
 * It specifies the strategy for session management, the provider details for Duende Identity Server,
 * and callbacks for managing JWT tokens and session data.
 */
export const authOptions: NextAuthOptions = {
  // Secret used for hashing/encrypting tokens
  secret: process.env.NEXTAUTH_SECRET as string,
  session: {
    strategy: 'jwt' // Session strategy set to JWT
  },
  providers: [
    // Duende Identity Server 6 provider configuration
    DuendeIdentityServer6({
      id: 'id-server', // Unique identifier for the identity server
      clientId: 'nextApp',
      clientSecret: 'secret',
      issuer: 'http://localhost:5000', // URL of the Duende Identity Server
      authorization: {params: {scope: 'openid profile auctionApp'}}, // OAuth scopes
      idToken: true // Indicates if id_token should be obtained
    })
  ],
  callbacks: {
    // JWT callback to enrich the token with profile and account information
    async jwt({token, profile, account}) {
      if (profile) {
        token.username = profile.username; // Add username to the JWT token
      }
      if (account) {
        token.access_token = account.access_token; // Add access token to the JWT token
      }
      return token;
    },
    // Session callback to add custom data to the session object
    async session({session, token}) {
      if (token) {
        session.user.username = token.username; // Add username to the session
      }
      return session;
    }
  }
};

// Handlers for HTTP GET and POST requests for authentication using NextAuth with the specified options.
const handler = NextAuth(authOptions);
export {handler as GET, handler as POST};