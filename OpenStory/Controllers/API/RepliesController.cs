using OpenStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OpenStory.Controllers.API
{
    public class RepliesController : ApiController
    {
        private ApplicationDbContext _context;

        public RepliesController()
        {
            _context = new ApplicationDbContext();
        }

        //GET /api/replies
        public IEnumerable<Reply> GetReplies()
        {
            return _context.Replies.ToList();
        }

        //GET /api/replies/{id}
        public Reply GetReply(int id)
        {
            Reply reply = _context.Replies.SingleOrDefault(r => r.Id == id);

            if (reply == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return reply;
        }

        // POST /api/reply
        [HttpPost]
        public Reply CreateReply(Reply reply)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _context.Replies.Add(reply);
            _context.SaveChanges();

            return reply;
        }

        // PUT api/reply/upvote
        public void UpVoteReply(int id)
        {
            Reply reply = _context.Replies.SingleOrDefault(r => r.Id == id);

            if (reply == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            reply.Likes += 1;
            _context.SaveChanges();
        }

        //PUT api/reply/downvote
        public void DownVoteReply(int id)
        {
            Reply reply = _context.Replies.SingleOrDefault(r => r.Id == id);

            if (reply == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            reply.Likes -= 1;
            _context.SaveChanges();
        }
        
    }
}
